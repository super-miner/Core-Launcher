using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class PersistentDataGroup : StoredDataGroup {
    [JsonInclude] public int ConfigVersion = 1;
    [JsonInclude] public bool SetupComplete = false;
    [JsonInclude] public string SteamExePath = "";
    [JsonInclude] public string SteamGamePath = "";
    [JsonInclude] public string SteamGameServerPath = "";
    [JsonInclude] public string AppDataPath = "";
    
    public override string GetPath() {
        return "PersistantData.json";
    }
}