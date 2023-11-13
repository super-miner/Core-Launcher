using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.ModIO.JsonStructures;
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
    
    private static readonly string _modsListUrl = "https://api.mod.io/v1/games/5289/mods?api_key={api_key}";
    private static readonly string _dependenciesListUrl = "https://api.mod.io/v1/games/5289/mods/{mod_id}/dependencies?api_key={api_key}";
    private static readonly Regex _localModsRegex = new Regex(@"CL_Mod_(\d+)_(.+)");
    
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
        switch (urlType) {
            case UrlType.ModsList:
                return _modsListUrl.Replace("{api_key}", ApiKey);
            case UrlType.DependenciesList:
                return _dependenciesListUrl.Replace("{api_key}", ApiKey).Replace("{mod_id}", $"{modInfo.Id}");
            default:
                return "";
        }
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

    public static ModInfo GetModInfo(int modId) {
        return ModsList.Mods.FirstOrDefault(modInfo => modInfo.Id == modId);
    }
    
    public static async Task DownloadMods(List<int> modsList) {
        for (int i = 0; i < modsList.Count; i++) {
            int modId = modsList[i];
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", (double)i / modsList.Count, $"Downloading {modInfo.Name}...");

            string localPath = modInfo.GetCachePath();
            if (FileUtil.DirectoryExists(localPath)) {
                continue;
            }
            
            string tempPath = $"{FileUtil.GetPath(PathType.AppData)}/Temp/ModTemp.zip";
            
            GD.Print($"Mod Manager: Downloading files for {modInfo.Name} ({modInfo.Id})...");
            
            await FetchManager.DownloadFile(modInfo.ModFile.Download.Url, tempPath);
            
            GD.Print($"Mod Manager: Finished downloading files for {modInfo.Name} ({modInfo.Id}), unzipping...");
            
            FileUtil.UnzipToDirectory(tempPath, localPath);
            FileUtil.DeleteFile(tempPath);
            
            GD.Print($"Mod Manager: Finished unzipping files for {modInfo.Name} ({modInfo.Id}).");
        }
    }

    public static void InstallMods(List<int> modsList) {
        for (int i = 0; i < modsList.Count; i++) {
            int modId = modsList[i];
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", (double)i / modsList.Count, $"Installing {modInfo.Name}...");

            string localPath = modInfo.GetCachePath();
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
        
        foreach (string filePath in FileUtil.GetDirectories(modsPath)) {
            Match match = _localModsRegex.Match(filePath);

            if (!match.Success) {
                continue;
            }
            
            string modNumberString = match.Groups[1].ToString();
            int.TryParse(modNumberString, out int modNumber);

            if (modsList.Contains(modNumber)) {
                continue;
            }
            
            string modName = match.Groups[2].ToString();
                    
            FileUtil.DeleteDirectory(filePath);
                    
            GD.Print($"Mod Manager: Deleted mod {modName} from {filePath}.");
        }
    }

    public static async Task<List<int>> GetDependencies(List<int> modsList) {
        List<int> result = new List<int>(modsList);
        
        foreach (int modId in modsList) {
            ModInfo modInfo = GetModInfo(modId);

            if (modInfo.HasDependencies) {
                DependencyListInfo dependenciesList = await modInfo.GetDependencies();
                
                foreach (DependencyInfo dependency in dependenciesList.Dependencies) {
                    if (!result.Contains(dependency.Id)) {
                        result.Add(dependency.Id);
                    }
                }
            }
        }

        if (result.Count > modsList.Count) {
            result = await GetDependencies(result);
        }
        
        return result;
    }
    
    private static void OnDeserializeStoredData() {
        SetApiKey(StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey);
    }
    
    private static void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey = ApiKey;
    }
}