using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI;

namespace CoreLauncher.Scripts;

public enum UrlType {
    ModsList
}

public static class ModManager {
    private static readonly string modsListUrl = "https://api.mod.io/v1/games/5289/mods?api_key={api_key}";
    
    public static ModsListInfo ModsList = null;
    public static string ApiKey = "";

    public static void Init() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
        
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
    }

    public static async void FetchModsList() {
        ModsList = null;
        
        string jsonString = await FetchUtil.Fetch(GetURL(UrlType.ModsList));
        
        ModsList = JsonSerializer.Deserialize<ModsListInfo>(jsonString);
    }
    
    public static string GetURL(UrlType urlType) {
        switch (urlType) {
            case UrlType.ModsList:
                return modsListUrl.Replace("{api_key}", ApiKey);
            default:
                return "";
        }
    }
    
    private static void OnDeserializeStoredData() {
        ApiKey = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey;
    }
    
    private static void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey = ApiKey;
    }

    private static void OnStoredDataDeserialized() {
        FetchModsList();
    }
}