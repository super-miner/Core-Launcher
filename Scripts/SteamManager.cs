using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CMDSearch;
using CoreLauncher.Scripts;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot.Collections;

namespace CoreLauncher.Scripts;

public static class SteamManager {
	public static void RunGame() {
		OS.Execute($"{FileManager.GetPath(PathType.Project)}/Commands/RunGame.bat", new [] {GetPath()}, new Godot.Collections.Array());
	}

	public static string GetPath() {
		return StoredDataManager.Data.SteamPath != "" ? StoredDataManager.Data.SteamPath : FileManager.GetPath(PathType.Steam);
	}

	public static void SetPath(string path) {
		StoredDataManager.Data.SteamPath = path;
		StoredDataManager.Serialize();
	}
}
