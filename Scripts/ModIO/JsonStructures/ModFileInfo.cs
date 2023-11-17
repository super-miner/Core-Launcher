using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModFileInfo {
    [JsonInclude] [JsonPropertyName("version")] public string Version;
    [JsonInclude] [JsonPropertyName("download")] public ModDownloadInfo Download;
}