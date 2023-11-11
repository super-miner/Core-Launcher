using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class DependencyInfo {
    [JsonInclude] [JsonPropertyName("mod_id")] public int Id;
}