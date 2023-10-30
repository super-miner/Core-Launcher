using Godot;
using System;
using Godot.Collections;

public static class SteamManager {
	public static void RunGame() {
		OS.Execute(ProjectSettings.GlobalizePath("res://") + "/Commands/RunGame.bat", new string[0], new Godot.Collections.Array());
	}
	
	public static string FindSteamPath() {
		object pathObject = RegistryManager.GetValue("SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath");
		
		if (pathObject is string pathString) {
			return pathString;
		}
		return "";
	}
}
