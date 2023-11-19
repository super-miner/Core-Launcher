using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.ModIO.LocalInfo;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.Systems.Fetch;
using Godot;

namespace CoreLauncher.Scripts.ModIO;

public enum UrlType {
    ModsList,
    DependenciesList
}

public delegate void OnModInfoLoaded();

public static class ModManager {
    public static event OnModInfoLoaded ModInfoLoadedEvent;
    
    public static bool HasLoaded = false;
    
    private static readonly string ModsListUrl = "https://api.mod.io/v1/games/5289/mods?api_key={api_key}";
    private static readonly string DependenciesListUrl = "https://api.mod.io/v1/games/5289/mods/{mod_id}/dependencies?api_key={api_key}";
    
    public static ModsListInfo ModsList = null;
    public static string ApiKey = "";

    public static void Init() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
    }

    public static async void FetchModsList() {
        ModsList = null;
        
        string jsonString = await FetchManager.FetchString(GetUrl(UrlType.ModsList));
        
        ModsList = JsonSerializer.Deserialize<ModsListInfo>(jsonString);
        await ModsList.Init();

        HasLoaded = true;
        ModInfoLoadedEvent?.Invoke();
    }
    
    public static string GetUrl(UrlType urlType, ModInfo modInfo = null) {
        string url = "";
        
        switch (urlType) {
            case UrlType.ModsList:
                url = ModsListUrl;
                break;
            case UrlType.DependenciesList:
                url = DependenciesListUrl;
                break;
        }

        url = url.Replace("{api_key}", $"{ApiKey}");
        if (modInfo != null) {
            url = url.Replace("{mod_id}", $"{modInfo.Id}");
        }

        return url;
    }

    public static ModInfo GetModInfo(int modId) {
        return ModsList.Mods.FirstOrDefault(modInfo => modInfo.Id == modId);
    }

    public static LocalModInfo GetLocalModInfo(int modId) {
        foreach (string directoryPath in FileUtil.GetDirectories(FileUtil.GetPath(PathType.ModCache))) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);

            if (localModInfo.Id == modId) {
                return localModInfo;
            }
        }

        return null;
    }
    
    public static void SetApiKey(string apiKey) {
        ApiKey = apiKey;

        if (!string.IsNullOrEmpty(ApiKey) && SetupManager.SetupComplete) {
            FetchModsList();
        }
    }
    
    public static async Task<bool> ValidateApiKey(string apiKey) {
        string tempApiKey = ApiKey;
        ApiKey = apiKey;
        
        HttpResponseMessage testMessageResponse = await FetchManager.Fetch(GetUrl(UrlType.ModsList));
        
        ApiKey = tempApiKey;
        
        if (testMessageResponse.StatusCode == HttpStatusCode.Unauthorized) {
            return false;
        }

        return true;
    }
    
    public static async Task<List<int>> GetDependencies(List<int> modsList) {
        List<int> result = new List<int>(modsList);
        
        foreach (int modId in modsList) {
            ModInfo modInfo = GetModInfo(modId);

            if (modInfo.HasDependencies) {
                List<int> dependenciesList = await modInfo.GetDependencies();
                
                foreach (int dependency in dependenciesList) {
                    if (!result.Contains(dependency)) {
                        result.Add(dependency);
                    }
                }
            }
        }

        if (result.Count > modsList.Count) {
            result = await GetDependencies(result);
        }
        
        return result;
    }

    public static void RemoveOldModVersions() {
        foreach (string directoryPath in FileUtil.GetDirectories(FileUtil.GetPath(PathType.ModCache))) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);
            
            if (localModInfo != null) {
                ModInfo modInfo = GetModInfo(localModInfo.Id);

                if (localModInfo.Version == modInfo.ModFile.Version) {
                    continue;
                }
            }
            
            GD.Print($"Mod Manager: Found an outdated mod in the cache ({directoryName}), removing it from {directoryPath}.");
                
            FileUtil.DeleteDirectory(directoryPath);
        }
    }
    
    public static async Task DownloadMods(List<int> modsList) {
        for (int i = 0; i < modsList.Count; i++) {
            int modId = modsList[i];
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", (double)i / modsList.Count, $"Downloading {modInfo.Name}...");

            string localPath = modInfo.GetLocalDirectoryPath();
            if (FileUtil.DirectoryExists(localPath)) {
                continue;
            }
            
            string tempPath = FileUtil.GetPath(PathType.ModTemp);
            
            GD.Print($"Mod Manager: Downloading files for {modInfo.Name} version {modInfo.ModFile.Version}..");
            
            await FetchManager.DownloadFile(modInfo.ModFile.Download.Url, tempPath);
            
            GD.Print($"Mod Manager: Finished downloading files for {modInfo.Name} version {modInfo.ModFile.Version}, unzipping to {localPath}...");
            
            FileUtil.UnzipToDirectory(tempPath, localPath);
            FileUtil.DeleteFile(tempPath); // TODO: Move this out of the for loop and see if it breaks
            
            GD.Print($"Mod Manager: Finished unzipping files for {modInfo.Name} version {modInfo.ModFile.Version} to {localPath}.");
        }
    }

    public static void InstallMods(List<int> modsList) {
        for (int i = 0; i < modsList.Count; i++) {
            int modId = modsList[i];
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", (double)i / modsList.Count, $"Installing {modInfo.Name}...");

            string localPath = modInfo.GetLocalDirectoryPath();
            if (!FileUtil.DirectoryExists(localPath)) {
                continue;
            }
            
            string installPath = $"{GameManager.GetModsPath()}/CL_Mod_{modInfo.Id}_{modInfo.Name}";
            
            GD.Print($"Mod Manager: Installing {modInfo.Name} from {localPath} to {installPath}.");
            
            FileUtil.CopyDirectory(localPath, installPath);

            GD.Print($"Mod Manager: Finished installing {modInfo.Name} from {localPath} to {installPath}.");
        }
    }
    
    public static void UninstallMods(List<int> modsList) {
        string modsPath = GameManager.GetModsPath();
        
        foreach (string directoryPath in FileUtil.GetDirectories(FileUtil.GetPath(PathType.ModCache))) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);
            
            if (localModInfo == null || modsList.Contains(localModInfo.Id)) {
                continue;
            }
                    
            FileUtil.DeleteDirectory(directoryPath);
                    
            GD.Print($"Mod Manager: Deleted mod {localModInfo.Name} from {directoryPath}.");
        }
    }

    public static async Task ManageMods(List<int> modsList) {
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Dependencies", 0.0, "Finding dependencies...");
			
        List<int> fullModsList = await GetDependencies(modsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Dependencies", 1.0, "Found dependencies.");
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModRemoval", 0.5, "Removing mods...");

        RemoveOldModVersions();
        UninstallMods(fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModRemoval", 1.0, "Removed mods.");
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", 0.0, "Downloading mods...");
			
        await DownloadMods(fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", 1.0, "Downloaded mods.");
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", 0.0, "Installing mods...");

        InstallMods(fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", 1.0, "Installed mods.");
    }

    public static string GetModLocalDirectoryName(int id, string name, string version) {
        return $"CL_Mod_{id}_{name.Replace(" ", "_")}_{version}";
    }

    public static string GetModLocalDirectoryPath(int id, string name, string version) {
        return $"{FileUtil.GetPath(PathType.ModCache)}{GetModLocalDirectoryName(id, name, version)}";
    }
    
    private static void OnDeserializeStoredData() {
        SetApiKey(StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey);
    }
    
    private static void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey = ApiKey;
    }
}