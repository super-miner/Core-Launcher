using Godot;
using System;
using CoreLauncher.Scripts;
using Godot.Collections;

namespace CoreLauncher.Scripts;

public static class SteamManager {
	public static void RunGame() {
		OS.Execute(FileManager.GetPath(PathType.Project) + "/Commands/RunGame.bat", new [] {GetPath()}, new Godot.Collections.Array());
	}

	public static string GetPath() {
		return FileManager.GetPath(PathType.Steam);
	}
}
