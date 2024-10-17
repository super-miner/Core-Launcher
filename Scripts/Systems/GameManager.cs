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
	public static string SteamGamePath;
	public static string SteamGameServerPath;
	public static string AppDataPath;
	
	public static void Init() {
		StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
		StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;

		if (StoredDataManager.HasDeserialized) {
			OnDeserializeStoredData();
		}
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

			switch (osName)
			{
				case "Windows":
					OS.Execute($"{FileUtil.GetPath(PathType.SteamExe)}/steam.exe", new string[] {"-applaunch", profileEntry.Profile.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
					break;
				case "Linux":
					OS.Execute($"{FileUtil.GetPath(PathType.SteamExe)}/steam", new string[] {"-applaunch", profileEntry.Profile.Server ? "1963720" : "1621690"}, new Godot.Collections.Array());
					break;
				default:
					GD.PrintErr($"Unrecognized operating system {osName}.");
					break;
			}
		}
		
		await Task.Delay(2000);
			
		InstanceManager.GetInstance<MainMenuManager>()?.PlayProgressBar.Reset();
	}

	public static string GetCoreKeeperPath() {
		return $"{FileUtil.GetPath(PathType.SteamGames)}";
	}
	
	public static string GetCoreKeeperServerPath() {
		return $"{FileUtil.GetPath(PathType.SteamGamesServer)}";
	}
	
	public static string GetModsPath(bool isServer = false) {
		if (isServer)
		{ return $@"{FileUtil.GetPath(PathType.SteamGamesServer)}\CoreKeeperServer_Data\StreamingAssets\Mods"; }
		return $@"{FileUtil.GetPath(PathType.SteamGamesServer)}\CoreKeeper_Data\StreamingAssets\Mods";
		
	}
	 
	public static string GetCoreKeeperDataPath(bool server) {
		return server ? $@"{GetCoreKeeperServerPath()}\CoreKeeperServer_Data\" : $@"{GetCoreKeeperPath()}\CoreKeeper_Data\";
	}

	public static string GetAppDataPath() {
		return $"{AppDataPath}/";
	}
	
	private static void OnDeserializeStoredData() {
		SteamExePath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamExePath;
		SteamGamePath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGamePath;
		SteamGameServerPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGameServerPath;
		AppDataPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().AppDataPath;
	}

	private static void OnSerializeStoredData() {
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamExePath = SteamExePath;
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGamePath = SteamGamePath;
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamGameServerPath = SteamGameServerPath;
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().AppDataPath = AppDataPath;
	}
}
