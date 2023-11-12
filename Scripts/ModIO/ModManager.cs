using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        if (!string.IsNullOrEmpty(ApiKey)) {
            FetchModsList();
        }
    }

    public static ModInfo GetModInfo(int modId) {
        return ModsList.Mods.FirstOrDefault(modInfo => modInfo.Id == modId);
    }
    
    public static async Task DownloadMods(List<int> modsList) {
        foreach (int modId in modsList) {
            ModInfo modInfo = GetModInfo(modId);
            
            InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Mods", 1.0, $"Downloading {modInfo.Name}...");

            if (!FileUtil.DirectoryExists(modInfo.GetCachePath())) {
                string tempPath = $"{FileUtil.GetPath(PathType.AppData)}/Temp/ModTemp.zip";
                
                GD.Print($"Mod Manager: Downloading files for {modInfo.Name} ({modInfo.Id})...");
                
                await FetchManager.DownloadFile(modInfo.ModFile.Download.Url, tempPath);
                
                GD.Print($"Mod Manager: Finished downloading files for {modInfo.Name} ({modInfo.Id}), unzipping...");
                
                FileUtil.UnzipToDirectory(tempPath, modInfo.GetCachePath());
                FileUtil.DeleteFile(tempPath);
                
                GD.Print($"Mod Manager: Finished unzipping files for {modInfo.Name} ({modInfo.Id}).");
            }
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