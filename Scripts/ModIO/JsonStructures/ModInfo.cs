using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModInfo {
    [JsonInclude] [JsonPropertyName("name")] public string Name;
    [JsonInclude] [JsonPropertyName("submitted_by")] public AuthorInfo Author;
    [JsonInclude] [JsonPropertyName("summary")] public string Summary;
    [JsonInclude] [JsonPropertyName("modfile")] public ModFileInfo ModFile;
    [JsonInclude] [JsonPropertyName("logo")] public LogoInfo Logo;

    public async Task Init() {
        await Logo.Init();
    }
}