using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts; 

public class ModFileInfo {
    [JsonInclude] [JsonPropertyName("download")] public ModDownloadInfo Download;
}