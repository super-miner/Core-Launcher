using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModFileInfo {
    [JsonInclude] [JsonPropertyName("download")] public ModDownloadInfo Download;
}