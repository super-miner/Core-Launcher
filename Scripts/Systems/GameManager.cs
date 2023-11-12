using System.Collections.Generic;
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
	public static readonly string ModsRelativePath = "/CoreKeeper_Data/StreamingAssets/Mods";
	
	public static string SteamPath;
	
	public static void Init() {
		StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
		StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
	}
	
	public static async void RunGame() {
		SelectableItemListEntry selectableEntry = MainMenuManager.Instance.ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			List<int> fullModsList = await ModManager.GetDependencies(profileEntry.Mods);
			
			ModManager.DownloadMods(fullModsList);
		}
		
		OS.Execute($"{FileUtil.GetPath(PathType.Project)}/Commands/RunGame.bat", new [] {GetSteamPath()}, new Godot.Collections.Array());
	}

	public static string GetSteamPath() {
		string steamPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamPath;
		return steamPath != "" ? steamPath : FileUtil.GetPath(PathType.Steam);
	}

	public static string GetCoreKeeperPath() {
		return GetSteamPath() + CoreKeeperRelativePath;
	}
	
	public static string GetModsPath() {
		return GetCoreKeeperPath() + ModsRelativePath;
	}
	
	private static void OnDeserializeStoredData() {
		SteamPath = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamPath;
	}

	private static void OnSerializeStoredData() {
		StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SteamPath = SteamPath;
	}
}
