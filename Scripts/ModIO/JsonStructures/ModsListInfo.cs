using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts; 

public class ModsListInfo {
    [JsonInclude] [JsonPropertyName("data")] public static List<ModInfo> Mods = null;
}