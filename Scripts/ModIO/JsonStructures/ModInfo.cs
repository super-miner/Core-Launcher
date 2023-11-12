using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Godot;

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
            string dependencyUrl = ModManager.GetUrl(UrlType.DependenciesList, this);
            
            GD.Print($"Mod Manager: Fetching dependencies from {dependencyUrl}.");
            
            string jsonString = await FetchUtil.FetchString(dependencyUrl);
        
            DependenciesList = JsonSerializer.Deserialize<DependencyListInfo>(jsonString);
            
            GD.Print($"Mod Manager: Fetched dependencies from {dependencyUrl}.");
        }
    }

    public string GetCachePath() {
        return $"{FileUtil.GetPath(PathType.AppData)}/ModCache/CL_Mod_{Id}_{Name.Replace(" ", "_")}";
    }
}