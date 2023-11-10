using System.Collections.Generic;
using System.Text.Json.Serialization;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;

namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class ProfileDataGroup : StoredDataGroup {
    [JsonInclude] public int SelectedEntry = -1;
    [JsonInclude] public List<StoredProfileListEntry> Profiles = new List<StoredProfileListEntry>();
    
    public override string GetPath() {
        return "Profiles.json";
    }
}