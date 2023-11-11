using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModInfo {
    [JsonInclude] [JsonPropertyName("name")] public string Name;
    [JsonInclude] [JsonPropertyName("id")] public int Id;
    [JsonInclude] [JsonPropertyName("submitted_by")] public AuthorInfo Author;
    [JsonInclude] [JsonPropertyName("summary")] public string Summary;
    [JsonInclude] [JsonPropertyName("modfile")] public ModFileInfo ModFile;
    [JsonInclude] [JsonPropertyName("logo")] public LogoInfo Logo;
    [JsonInclude] [JsonPropertyName("dependencies")] public bool HasDependencies;
    public DependencyListInfo DependenciesList;

    public async Task Init() {
        await Logo.Init();

        if (HasDependencies) {
            
        }
    }

    public string GetCachePath() {
        return $"{FileUtil.GetPath(PathType.AppData)}/ModCache/CL_Mod_{Id}_{Name.Replace(" ", "_")}";
    }
}