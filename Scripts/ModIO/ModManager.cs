using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts.ModIO;

public enum UrlType {
    ModsList
}

public delegate void OnModInfoLoaded();

public static class ModManager {
    public static event OnModInfoLoaded ModInfoLoadedEvent;
    
    private static readonly string _modsListUrl = "https://api.mod.io/v1/games/5289/mods?api_key={api_key}";
    
    public static ModsListInfo ModsList = null;
    public static string ApiKey = "";

    public static void Init() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
    }

    public static async void FetchModsList() {
        ModsList = null;
        
        string jsonString = await FetchUtil.FetchString(GetUrl(UrlType.ModsList));
        
        ModsList = JsonSerializer.Deserialize<ModsListInfo>(jsonString);
        await ModsList.Init();
        
        ModInfoLoadedEvent?.Invoke();
    }
    
    public static string GetUrl(UrlType urlType) {
        switch (urlType) {
            case UrlType.ModsList:
                return _modsListUrl.Replace("{api_key}", ApiKey);
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
    
    public static async void DownloadMods(List<int> modsList) {
        foreach (int modId in modsList) {
            ModInfo modInfo = GetModInfo(modId);

            if (!FileUtil.DirectoryExists(modInfo.GetCachePath())) {
                string tempPath = $"{FileUtil.GetPath(PathType.AppData)}/Temp/ModTemp.zip";
                
                GD.Print($"Mod Manager: Downloading files for {modInfo.Name} ({modInfo.Id})...");
                
                await FetchUtil.DownloadFile(modInfo.ModFile.Download.Url, tempPath);
                
                GD.Print($"Mod Manager: Finished downloading files for {modInfo.Name} ({modInfo.Id}), unzipping...");
                
                FileUtil.UnzipToDirectory(tempPath, modInfo.GetCachePath());
                
                GD.Print($"Mod Manager: Finished unzipping files for {modInfo.Name} ({modInfo.Id}).");
            }
        }
    }

    public static List<int> GetDependencies(List<int> modsList) {
        List<int> result = modsList;
        
        foreach (int modId in modsList) {
            ModInfo modInfo = GetModInfo(modId);

            if (modInfo.HasDependencies) {
                foreach (DependencyInfo dependency in modInfo.DependenciesList.Dependencies) {
                    if (!result.Contains(dependency.Id)) {
                        result.Add(dependency.Id);
                    }
                }
            }
        }

        if (result.Count > modsList.Count) {
            result = GetDependencies(result);
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