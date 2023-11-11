using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts; 

public class ModDownloadInfo {
    [JsonInclude] [JsonPropertyName("binary_url")] public string DownloadUrl;
}