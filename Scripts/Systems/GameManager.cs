using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Systems;

public static class GameManager {
	public static readonly string CoreKeeperRelativePath = "/steamapps/common/Core Keeper";
	public static readonly string CoreKeeperServerRelativePath = "/steamapps/common/Core Keeper Dedicated Server";
	public static readonly string ModsRelativePath = "/CoreKeeper_Data/StreamingAssets/Mods";
	public static readonly string ModsRelativeServerPath = "/CoreKeeperServer_Data/StreamingAssets/Mods";
	
	public static string SteamPath;
	
	public static void Init() {
		StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
		StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
	}
	
	public static async void RunGame() {
		SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();

		if (selectableEntry == null) {
			InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.SetValue("Dependencies", 0.0, "No profile was selected...");
			return;
		}
		
		if (selectableEntry is ProfileListEntry profileEntry) {
			await ModManager.ManageMods(profileEntry.Server, profileEntry.Mods);

			string osName = OS.GetName();

			/*if (osName == "Windows") {
				OS.Execute("steam", new [] {"-applaunch", profileEntry.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
			}
			else if (osName == "Linux") {
				OS.Execute("steam", new string[] {"-applaunch", profileEntry.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
			}
			else {
				GD.PrintErr($"Unrecognized operating system {osName}.");
			}*/
			
			OS.ShellOpen($"steam://rungameid/{(profileEntry.Server ? 1963720 : 1621690)}");
		}
		
		await Task.Delay(2000);
			
		InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.Reset();
	}

	public static string GetCoreKeeperPath() {
		return FileUtil.GetPath(PathType.Steam) + CoreKeeperRelativePath;
	}
	
	public static string GetCoreKeeperServerPath() {
		return FileUtil.GetPath(PathType.Steam) + CoreKeeperServerRelativePath;
	}
	
	public static string GetModsPath(bool server) {
		return server ? GetCoreKeeperServerPath() + ModsRelativeServerPath : GetCoreKeeperPath() + ModsRelativePath;
	}
	
	private static void OnDeserializeStoredData() {
		SteamPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamPath;
	}

	private static void OnSerializeStoredData() {
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamPath = SteamPath;
	}
}
