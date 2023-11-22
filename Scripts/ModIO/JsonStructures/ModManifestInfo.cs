using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModManifestInfo {
    [JsonInclude] [JsonPropertyName("skipSafetyChecks")] public bool SkipSafetyChecks;
}