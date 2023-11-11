using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class LogoInfo {
    [JsonInclude] [JsonPropertyName("original")] public string LogoUrl;
    public Image LogoImage;

    public async Task Init() {
        GD.Print($"LogoInfo: Downloading mod logo from {LogoUrl}.");
        
        LogoImage = await FetchUtil.FetchImage(LogoUrl);
        
        GD.Print($"LogoInfo: Finished downloading mod logo from {LogoUrl}.");
    }
}