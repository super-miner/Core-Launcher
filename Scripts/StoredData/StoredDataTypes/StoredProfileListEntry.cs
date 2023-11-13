using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData.StoredDataTypes; 

public class StoredProfileListEntry {
    [JsonInclude] public string Name = "";
    [JsonInclude] public List<int> Mods = new List<int>();
}