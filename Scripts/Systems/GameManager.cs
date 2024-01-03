using System.Threading.Tasks;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Systems;

public static class GameManager {
	public static string SteamExePath;
	public static string SteamGamesPath;
	public static string AppDataPath;
	
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
			if (string.IsNullOrEmpty(profileEntry.GetName())) {
				TaskCompletionSource<string> popupTask = new TaskCompletionSource<string>();
				
				InstanceManager.GetInstance<MainMenuManager>().NameProfilePopup.Open(popupTask);
				
				string popupResult = await popupTask.Task;

				if (string.IsNullOrEmpty(popupResult)) {
					return;
				}
				
				profileEntry.SetName(popupResult);
			}

			await profileEntry.Profile.Install();

			string osName = OS.GetName();

			if (osName == "Windows") {
				OS.Execute($"{FileUtil.GetPath(PathType.SteamExe)}/steam.exe", new string[] {"-applaunch", profileEntry.Profile.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
			}
			else if (osName == "Linux") {
				OS.Execute($"{FileUtil.GetPath(PathType.SteamExe)}/steam", new string[] {"-applaunch", profileEntry.Profile.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
			}
			else {
				GD.PrintErr($"Unrecognized operating system {osName}.");
			}
		}
		
		await Task.Delay(2000);
			
		InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.Reset();
	}

	public static string GetCoreKeeperPath() {
		return $"{FileUtil.GetPath(PathType.SteamGames)}/steamapps/common/Core Keeper/";
	}
	
	public static string GetCoreKeeperServerPath() {
		return $"{FileUtil.GetPath(PathType.SteamGames)}/steamapps/common/Core Keeper Dedicated Server/";
	}
	
	public static string GetCoreKeeperDataPath(bool server) {
		return server ? $"{GetCoreKeeperServerPath()}CoreKeeperServer_Data/" : $"{GetCoreKeeperPath()}CoreKeeper_Data/";
	}

	public static string GetAppDataPath() {
		return $"{AppDataPath}/";
	}
	
	private static void OnDeserializeStoredData() {
		SteamExePath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamExePath;
		SteamGamesPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGamesPath;
		AppDataPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().AppDataPath;
	}

	private static void OnSerializeStoredData() {
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamExePath = SteamExePath;
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGamesPath = SteamGamesPath;
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().AppDataPath = AppDataPath;
	}
}
