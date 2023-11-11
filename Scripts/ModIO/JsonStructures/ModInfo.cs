using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts; 

public class ModInfo {
    [JsonInclude] [JsonPropertyName("name")] public string Name;
    [JsonInclude] [JsonPropertyName("submitted_by")] public AuthorInfo Author;
    [JsonInclude] [JsonPropertyName("summary")] public string Summary;
    [JsonInclude] [JsonPropertyName("modfile")] public ModFileInfo ModFile;
}