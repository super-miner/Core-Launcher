using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class PersistentDataGroup : StoredDataGroup {
    [JsonInclude] public bool SetupComplete = false;
    [JsonInclude] public string SteamPath = "";
    
    public override string GetPath() {
        return "PersistantData.json";
    }
}