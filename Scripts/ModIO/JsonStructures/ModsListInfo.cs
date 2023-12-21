using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Menus.Loading;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModsListInfo {
	[JsonInclude] [JsonPropertyName("data")] public List<ModInfo> Mods = null;

	public async Task Init() {
		for (int i = 0; i < Mods.Count; i++) {
			ModInfo mod = Mods[i];
			
			InstanceManager.GetInstance<LoadingManager>().ProgressBar.SetValue("Mods", (double)i / ModManager.ModsList.Mods.Count, $"Loading data for mod {mod.Name}...");
			
			await mod.Init();
		}
	}
}
