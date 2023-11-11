using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModDownloadInfo {
    [JsonInclude] [JsonPropertyName("binary_url")] public string DownloadUrl;
}