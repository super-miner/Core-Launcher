using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class DependencyListInfo {
    [JsonInclude] [JsonPropertyName("data")] public List<DependencyInfo> Dependencies = null;
}