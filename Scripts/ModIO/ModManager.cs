using System;
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
    
    private static readonly string ModsListUrl = "https://g-5289.modapi.io/v1/games/5289/mods?api_key=40460f9354653502fc6e4666f9f10cce";
    private static readonly string DependenciesListUrl = "https://g-5289.modapi.io/v1/games/5289/mods/{mod_id}/dependencies?api_key=40460f9354653502fc6e4666f9f10cce";
    
    public static ModsListInfo ModsList = null;

    public static async Task FetchModsList() {
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
        
        if (modInfo != null) {
            url = url.Replace("{mod_id}", $"{modInfo.Id}");
        }

        return url;
    }

    public static ModInfo GetModInfo(int modId) {
        return ModsList.Mods.FirstOrDefault(modInfo => modInfo.Id == modId);
    }
    
    public static ModInfo GetModInfo(string modName) {
        return ModsList.Mods.FirstOrDefault(modInfo => modInfo.Name == modName);
    }

    public static LocalModInfo GetLocalModInfo(int modId, string modsDirectoryPath) {
        foreach (string directoryPath in FileUtil.GetDirectories(modsDirectoryPath)) {
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryPath);

            if (localModInfo.Id == modId) {
                return localModInfo;
            }
        }

        return null;
    }

    public static (List<int>, List<int>) GetModDeltas(List<int> modsListOld, List<int> modsListNew) {
        List<int> addedMods = new List<int>();
        List<int> removedMods = new List<int>();

        foreach (int modId in modsListNew) {
            if (!modsListOld.Contains(modId)) {
                addedMods.Add(modId);
            }
        }
        
        foreach (int modId in modsListOld) {
            if (!modsListNew.Contains(modId)) {
                removedMods.Add(modId);
            }
        }

        return (addedMods, removedMods);
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
    
    public static async Task DownloadMod(int modId, string path) {
        ModInfo modInfo = GetModInfo(modId);

        string downloadPath = $"{path}{GetModLocalDirectoryName(modId, modInfo.Name, modInfo.ModFile.Version)}";
        string tempPath = FileUtil.GetPath(PathType.ModTemp);
        
        GD.Print($"Mod Manager: Downloading files for {modInfo.Name} version {modInfo.ModFile.Version}..");
        
        await FetchManager.DownloadFile(modInfo.ModFile.Download.Url, tempPath);
        
        GD.Print($"Mod Manager: Finished downloading files for {modInfo.Name} version {modInfo.ModFile.Version}, unzipping to {downloadPath}...");
        
        FileUtil.CreateDirectory(downloadPath);
        FileUtil.UnzipToDirectory(tempPath, downloadPath);
        FileUtil.DeleteFile(tempPath);
        
        GD.Print($"Mod Manager: Finished unzipping files for {modInfo.Name} version {modInfo.ModFile.Version} to {downloadPath}.");
    }

    public static void RemoveMod(int modId, string path) {
        LocalModInfo localModInfo = GetLocalModInfo(modId, path);
        FileUtil.DeleteDirectory(localModInfo.Path);
    }
    
    public static bool IsModOutdated(LocalModInfo localModInfo) {
        ModInfo modInfo = GetModInfo(localModInfo.Id);
        return localModInfo.Version != modInfo.ModFile.Version;
    }
    
    public static bool IsModSafe(LocalModInfo localModInfo) {
        try {
            ModManifestInfo modManifest = FileUtil.ReadJsonFile<ModManifestInfo>($"{localModInfo.Path}/ModManifest.json");

            ModInfo modInfo = GetModInfo(localModInfo.Id);

            return !(modManifest.SkipSafetyChecks && !modInfo.IsElevatedAccess());
        }
        catch (Exception exception) {
            return false;
        }
    }

    /*public static void RemoveOldModVersions() {
        foreach (string directoryPath in FileUtil.GetDirectories(FileUtil.GetPath(PathType.ModCache))) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);
            
            if (localModInfo != null) {
                ModInfo modInfo = GetModInfo(localModInfo.Id);

                if (localModInfo.Version == modInfo.ModFile.Version) {
                    continue;
                }
                else {
                    GD.Print($"Mod Manager: Mod likely outdated ({localModInfo.Version} != {modInfo.ModFile.Version})...");
                }
            }
            else {
                GD.Print($"Mod Manager: Could not discern local mod info from {directoryName}.");
            }
            
            GD.Print($"Mod Manager: Found an outdated mod in the cache ({directoryName}), removing it from {directoryPath}.");
                
            FileUtil.DeleteDirectory(directoryPath);
        }
    }*/
    
    /*public static async Task DownloadMods(List<int> modsList) {
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
    }*/

    /*public static void RemoveUnsafeMods() {
        foreach (string directoryPath in FileUtil.GetDirectories(FileUtil.GetPath(PathType.ModCache))) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);
            
            try {
                ModManifestInfo modManifest = FileUtil.ReadJsonFile<ModManifestInfo>($"{directoryPath}/ModManifest.json");

                if (modManifest.SkipSafetyChecks) {
                    GD.Print(
                        $"Mod Manager: The skipSafetyChecks value for {localModInfo.Name} was set to true, deleting it.");
                    FileUtil.DeleteDirectory(directoryPath);
                }
            }
            catch (Exception exception) {
                GD.Print(
                    $"Mod Manager: The ModManifest.json for {localModInfo.Name} could not be found/parsed, deleting it.");
                FileUtil.DeleteDirectory(directoryPath);
            }
        }
    }*/

    /*public static void InstallMods(bool server, List<int> modsList) {
        for (int i = 0; i < modsList.Count; i++) {
            int modId = modsList[i];
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", (double)i / modsList.Count, $"Installing {modInfo.Name}...");

            string localPath = modInfo.GetLocalDirectoryPath();
            if (!FileUtil.DirectoryExists(localPath)) {
                continue;
            }
            
            string installPath = $"{GameManager.GetModsPath(server)}/CL_Mod_{modInfo.Id}_{modInfo.Name}_{modInfo.ModFile.Version}";
            
            GD.Print($"Mod Manager: Installing {modInfo.Name} from {localPath} to {installPath}.");
            
            FileUtil.CopyDirectory(localPath, installPath);

            GD.Print($"Mod Manager: Finished installing {modInfo.Name} from {localPath} to {installPath}.");
        }
    }
    
    public static void UninstallMods(bool server, List<int> modsList) {
        string modsPath = GameManager.GetModsPath(server);
        
        foreach (string directoryPath in FileUtil.GetDirectories(modsPath)) {
            string directoryName = FileUtil.GetDirectoryName(directoryPath);
            LocalModInfo localModInfo = LocalModInfo.FromString(directoryName);
            
            if (localModInfo == null || modsList.Contains(localModInfo.Id)) {
                continue;
            }
                    
            FileUtil.DeleteDirectory(directoryPath);
                    
            GD.Print($"Mod Manager: Deleted mod {localModInfo.Name} from {directoryPath}.");
        }
    }

    public static async Task ManageMods(bool server, List<int> modsList) {
        if (!FileUtil.DirectoryExists(FileUtil.GetPath(PathType.ModCache))) {
            FileUtil.CreateDirectory(FileUtil.GetPath(PathType.ModCache));
        }

        if (!FileUtil.DirectoryExists(GameManager.GetModsPath(server))) {
            FileUtil.CreateDirectory(GameManager.GetModsPath(server));
        }
        
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Dependencies", 0.0, "Finding dependencies...");
			
        List<int> fullModsList = await GetDependencies(modsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Dependencies", 1.0, "Found dependencies.");
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModRemoval", 0.5, "Removing mods...");

        RemoveOldModVersions();
        UninstallMods(server, fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModRemoval", 1.0, "Removed mods.");
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", 0.0, "Downloading mods...");
			
        await DownloadMods(fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModDownloads", 1.0, "Downloaded mods.");
        
        RemoveUnsafeMods();
        
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", 0.0, "Installing mods...");

        InstallMods(server, fullModsList);
			
        InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("ModInstalls", 1.0, "Installed mods.");
    }*/
    
    public static string GetModLocalDirectoryName(int id, string name, string version) {
        return $"CL_Mod_{id}_{name.Replace(" ", "_")}_{version}";
    }

    /*public static string GetModLocalDirectoryPath(int id, string name, string version) {
        return $"{FileUtil.GetPath(PathType.ModCache)}{GetModLocalDirectoryName(id, name, version)}";
    }*/
}