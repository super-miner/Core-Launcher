using System.IO;
using System.Text.Json;
using Godot;

namespace CoreLauncher.Scripts;

public enum PathType {
    AppData,
    StoredData,
    Project,
    Steam
}

public static class FileManager {
    public static string GetPath(PathType pathType) {
        switch (pathType) {
            case PathType.AppData:
                return ProjectSettings.GlobalizePath("user://") + "/CoreLauncher";
            case PathType.StoredData:
                return GetPath(PathType.AppData) + "/Data";
            case PathType.Project:
                return ProjectSettings.GlobalizePath("res://");
            case PathType.Steam:
                object pathObject = RegistryManager.GetValue("SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath");
		
                if (pathObject is string pathString) {
                    return pathString;
                }
                GD.PrintErr("Could not find steam path in the registry.");
                return "";
            default:
                GD.PrintErr($"The case for GetPath({pathType.ToString()} has not been implemented.");
                return "";
        }
    }

    public static void WriteTextFile(string path, string text) {
        File.WriteAllText(path, text);
    }
    
    public static void WriteJSONFile(string path, object data) {
        string jsonString = JsonSerializer.Serialize(data);
        
        FileManager.WriteTextFile(path, jsonString);
    }
}