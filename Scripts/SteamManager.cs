using Godot;
using System;

public static class SteamManager
{
	public static string FindSteamPath() {
		object pathObject = RegistryManager.GetValue("SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath");
		
		if (pathObject is string pathString) {
			return pathString;
		}
		return "";
	}
}
