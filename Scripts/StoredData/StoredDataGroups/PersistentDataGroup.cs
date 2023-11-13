using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class PersistentDataGroup : StoredDataGroup {
    [JsonInclude] public bool SetupComplete = false;
    [JsonInclude] public string SteamPath = "";
    [JsonInclude] public string ModIOApiKey = "";
    [JsonInclude] public string ModIOUserID = "";
    
    public override string GetPath() {
        return "PersistantData.json";
    }
}