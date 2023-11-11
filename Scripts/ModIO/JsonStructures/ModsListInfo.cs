using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModsListInfo {
    [JsonInclude] [JsonPropertyName("data")] public List<ModInfo> Mods = null;

    public async Task Init() {
        foreach (ModInfo mod in Mods) {
            await mod.Init();
        }
    }
}