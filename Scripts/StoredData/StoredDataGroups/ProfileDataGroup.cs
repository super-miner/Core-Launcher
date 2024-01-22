using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class ProfileDataGroup : StoredDataGroup {
    [JsonInclude] public string LastLoadedIntId = "";
    [JsonInclude] public string LastLoadedIntServerId = "";

    public override string GetPath() {
        return "ProfileInfo.json";
    }
}