using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class TagInfo {
    [JsonInclude] [JsonPropertyName("name")] public string Name;
}