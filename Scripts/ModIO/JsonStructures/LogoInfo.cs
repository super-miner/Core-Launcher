using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Systems.Fetch;
using Godot;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class LogoInfo {
    [JsonInclude] [JsonPropertyName("thumb_320x180")] public string LogoUrl;
    public Image LogoImage;

    public async Task Init() {
        GD.Print($"Mod Manager: Downloading mod logo from {LogoUrl}.");
        
        LogoImage = await FetchManager.FetchImage(LogoUrl);
        
        GD.Print($"Mod Manager: Finished downloading mod logo from {LogoUrl}.");
    }
}