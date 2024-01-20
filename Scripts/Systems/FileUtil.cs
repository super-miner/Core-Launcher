using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using Godot;
using Environment = System.Environment;

namespace CoreLauncher.Scripts.Systems;

public enum PathType {
    AppData,
    Project,
    SteamExe,
    SteamGames,
    SteamGamesServer,
    CoreKeeperAppData,
    Profiles,
    ModTemp
}

public static class FileUtil {
    public static string GetPath(PathType pathType) {
        switch (pathType) {
            case PathType.AppData:
                return ProjectSettings.GlobalizePath("user://");
            case PathType.Project:
                return ProjectSettings.GlobalizePath("res://");
            case PathType.SteamExe:
                if (!string.IsNullOrEmpty(GameManager.SteamExePath)) {
                    return GameManager.SteamExePath;
                }
                else {
                    string osName = OS.GetName();
                    
                    if (osName == "Windows") {
                        object pathObject = RegistryUtil.GetValue("SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath");
		
                        if (pathObject is string pathString) {
                            return pathString;
                        }
                        
                        GD.PrintErr("Could not find steam path in the registry.");
                        return "";
                    }
                    else if (osName == "Linux") {
                        return "~/.steam/steam";
                    }
                    else {
                        GD.PrintErr($"Unrecognized operating system {osName}.");
                    }

                    return "";
                }
            case PathType.SteamGames:
                if (!string.IsNullOrEmpty(GameManager.SteamGamesPath)) {
                    return GameManager.SteamGamesPath;
                }
                else {
                    string osName = OS.GetName();
                    
                    if (osName == "Windows") {
                        object pathObject = RegistryUtil.GetValue("SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath");
		
                        if (pathObject is string pathString) {
                            return pathString;
                        }
                        
                        GD.PrintErr("Could not find steam path in the registry.");
                        return "";
                    }
                    else if (osName == "Linux") {
                        Godot.Collections.Array output = new Godot.Collections.Array();
                        int success = OS.Execute("bash", new string[] {"-c", "which", "steam"}, output);

                        if (success >= 0 && output.Count > 0) {
                            return output[0].AsString();
                        }

                        GD.PrintErr("There was an error executing the \"which steam\" command. Could not find steam path.");
                        return "";
                    }
                    else {
                        GD.PrintErr($"Unrecognized operating system {osName}.");
                    }

                    return "";
                }
            case PathType.SteamGamesServer:
                if (!string.IsNullOrEmpty(GameManager.SteamGamesPath)) {
                    return GameManager.SteamGamesPath;
                }
                else {
                    return GetPath(PathType.SteamGames);
                }
            case PathType.CoreKeeperAppData:
                if (!string.IsNullOrEmpty(GameManager.AppDataPath)) {
                    return GameManager.AppDataPath;
                }
                else {
                    string intermediatePath = $"{GetParentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))}\\LocalLow\\Pugstorm\\Core Keeper\\Steam";
                    
                    return GetDirectories(intermediatePath).FirstOrDefault(directoryPath => {
                        string directoryName = GetDirectoryName(directoryPath);
                        return directoryName != "unknown";
                    }, "");
                }
            case PathType.Profiles:
                return $"{GetPath(PathType.AppData)}Profiles/";
            case PathType.ModTemp:
                return $"{GetPath(PathType.AppData)}Temp/ModTemp.zip";
            default:
                GD.PrintErr($"The case for GetPath({pathType.ToString()}) has not been implemented.");
                return "";
        }
    }

    public static void WriteTextFile(string path, string text) {
        FileInfo fileInfo = new FileInfo(path);
        DirectoryInfo fileDirectory = fileInfo.Directory; // TODO: Make this use FileUtil functions
        if (fileDirectory != null && !fileDirectory.Exists) {
            fileDirectory.Create();
        }
        
        File.WriteAllText(path, text);
    }
    
    public static void WriteJSONFile(string path, object data) {
        JsonSerializerOptions options = new JsonSerializerOptions {
            WriteIndented = true
        };
        string jsonString = JsonSerializer.Serialize(data, options);
        
        FileUtil.WriteTextFile(path, jsonString);
    }

    public static string ReadTextFile(string path) {
        return File.ReadAllText(path);
    }

    public static T ReadJsonFile<T>(string path) {
        string jsonString = ReadTextFile(path);

        return JsonSerializer.Deserialize<T>(jsonString);
    }
    
    public static object ReadJsonFile(Type returnType, string path) {
        string jsonString = ReadTextFile(path);

        return JsonSerializer.Deserialize(jsonString, returnType);
    }

    public static void DeleteFile(string path) {
        File.Delete(path);
    }
    
    public static void DeleteDirectory(string path) {
        Directory.Delete(path, true);
    }

    public static bool DirectoryContains(string path, string file) {
        return File.Exists($"{path}/{file}");
    }
    
    public static bool DirectoryExists(string path) {
        return Directory.Exists(path);
    }

    public static void CreateDirectory(string path) {
        Directory.CreateDirectory(path);
    }
    
    public static bool FileExists(string path) {
        return File.Exists(path);
    }

    public static void CopyFile(string fromPath, string toPath) {
        File.Copy(fromPath, toPath, true);
    }
    
    public static void CopyDirectory(string fromPath, string toPath) {
        if (!DirectoryExists(toPath)) {
            CreateDirectory(toPath);
        }
        
        foreach (string directoryPath in Directory.GetDirectories(fromPath, "*", SearchOption.AllDirectories)) {
            Directory.CreateDirectory(directoryPath.Replace(fromPath, toPath));
        }
        
        foreach (string filePath in Directory.GetFiles(fromPath, "*.*",SearchOption.AllDirectories)) {
            CopyFile(filePath, filePath.Replace(fromPath, toPath));
        }
    }

    public static string GetParentDirectory(string path) {
        DirectoryInfo directory = new DirectoryInfo(path);
        DirectoryInfo parentDirectory = directory.Parent;
        return parentDirectory != null ? parentDirectory.ToString() : null;
    }

    public static string[] GetFiles(string path) {
        return Directory.GetFiles(path);
    }

    public static string[] GetDirectories(string path) {
        return Directory.GetDirectories(path);
    }

    public static string GetFileName(string path) {
        return Path.GetFileName(path);
    }
    
    public static string GetDirectoryName(string path) {
        return Path.GetFileName(path.TrimEnd('/', '\\'));
    }

    public static void UnzipToDirectory(string zipPath, string directoryPath) {
        if (!DirectoryExists(directoryPath)) {
            CreateDirectory(directoryPath);
        }
        
        ZipFile.ExtractToDirectory(zipPath, directoryPath, Encoding.Default, true);
    }
}