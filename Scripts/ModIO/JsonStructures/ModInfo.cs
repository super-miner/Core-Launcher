using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.Systems.Fetch;
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
    
    private DependencyListInfo _dependenciesList;

    public async Task Init() {
        await Logo.Init();
    }

    public async Task<DependencyListInfo> GetDependencies() {
        if (HasDependencies) {
            if (_dependenciesList == null) {
                string dependencyUrl = ModManager.GetUrl(UrlType.DependenciesList, this);
            
                string jsonString = await FetchManager.FetchString(dependencyUrl);
                _dependenciesList = JsonSerializer.Deserialize<DependencyListInfo>(jsonString);
            }
            
            return _dependenciesList;
        }

        return null;
    }

    public override string ToString() {
        return ModManager.GetModLocalDirectoryName(Id, Name, ModFile.Version);
    }
    
    public string GetLocalDirectoryPath() {
        return ModManager.GetModLocalDirectoryPath(Id, Name, ModFile.Version);
    }
}