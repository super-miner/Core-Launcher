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
    ModTemp,
    GameApp,
    ServerApp
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
                        object pathObject = RegistryUtil.GetValue(@"SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath");
		
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
                if (!string.IsNullOrEmpty(GameManager.SteamGamePath)) {
                    return GameManager.SteamGamePath;
                }
                else {
                    string osName = OS.GetName();
                    
                    switch (osName)
                    {
                        case "Windows":
                        {
                            object pathObject = RegistryUtil.GetValue(@"SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath");
		
                            if (pathObject is string pathString) {
                                return pathString;
                            }
                        
                            GD.PrintErr("Could not find steam path in the registry.");
                            return "";
                        }
                        case "Linux":
                        {
                            Godot.Collections.Array output = new Godot.Collections.Array();
                            int success = OS.Execute("bash", new string[] {"-c", "which", "steam"}, output);

                            if (success >= 0 && output.Count > 0) {
                                return output[0].AsString();
                            }

                            GD.PrintErr("There was an error executing the \"which steam\" command. Could not find steam path.");
                            return "";
                        }
                        default:
                            GD.PrintErr($"Unrecognized operating system {osName}.");
                            break;
                    }

                    return "";
                }
            case PathType.SteamGamesServer:
                if (!string.IsNullOrEmpty(GameManager.SteamGameServerPath)) {
                    return GameManager.SteamGameServerPath;
                }
                else {
                    return GetPath(PathType.SteamGames);
                }
            case PathType.CoreKeeperAppData:
                if (!string.IsNullOrEmpty(GameManager.AppDataPath)) {
                    return GameManager.AppDataPath;
                }
                else {
                    string intermediatePath = $@"{GetParentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))}\LocalLow\Pugstorm\Core Keeper\Steam";
                    
                    return GetDirectories(intermediatePath).FirstOrDefault(directoryPath => {
                        string directoryName = GetDirectoryName(directoryPath);
                        return directoryName != "unknown";
                    }, "");
                }
            case PathType.Profiles:
                return $"{GetPath(PathType.AppData)}Profiles/";
            case PathType.ModTemp:
                return $"{GetPath(PathType.AppData)}Temp/ModTemp.zip";
            case PathType.GameApp:
            {
                var pathObject = RegistryUtil.GetValue(@"SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath");
                if (pathObject is string pathString)
                {
                    var libraryData = SteamLibraryParser.ReadSteamLibraryFile(pathString+@"\steamapps\libraryfolders.vdf");
                    foreach (var entry in libraryData)
                    {
                        foreach (var appId in entry.Value.Where(appId => appId == "1621690"))
                        {
                            GD.Print($@"Found {appId} in {entry.Key}!");
                            return entry.Key.Replace(@"\\", @"\");
                        }
                    }
                }
                
                GD.PrintErr("Could not find game path.");
                return "";
            }
            case PathType.ServerApp:
            {
                var pathObject = RegistryUtil.GetValue(@"SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath");
                if (pathObject is string pathString) {
                    var libraryData = SteamLibraryParser.ReadSteamLibraryFile(pathString+@"\steamapps\libraryfolders.vdf");
                    foreach (var entry in libraryData)
                    {
                        foreach (var appId in entry.Value.Where(appId => appId == "1963720"))
                        {
                            // Core Keeper Dedicated Server
                            GD.Print($@"Found {appId} in {entry.Key}!");
                            return entry.Key.Replace(@"\\", @"\");
                        }
                    }
                }
                
                GD.PrintErr("Could not find game path.");
                return "";
            }

            default:
                GD.PrintErr($"The case for GetPath({pathType.ToString()}) has not been implemented.");
                return "";
        }
    }

    /// <summary>
    /// Serializes an object to a JSON string and writes it to a specified file path.
    /// </summary>
    /// <param name="path">The file path where the JSON content will be written.</param>
    /// <param name="text">The object to be serialized to JSON format.</param>
    /// <remarks>
    /// Utilizes the <see cref="JsonSerializer"/> with indented formatting for readability.
    /// Calls <see cref="FileUtil.WriteTextFile"/> to handle the actual file writing operation.
    /// </remarks>
    public static void WriteTextFile(string path, string text) {
        FileInfo fileInfo = new FileInfo(path);
        DirectoryInfo fileDirectory = fileInfo.Directory; // TODO: Make this use FileUtil functions
        if (fileDirectory is { Exists: false }) {
            fileDirectory.Create();
        }
        
        File.WriteAllText(path, text);
    }

    /// <summary>
    /// Serializes the given object to a JSON string and writes it to the specified file path.
    /// </summary>
    /// <param name="path">The file path where the JSON content will be written.</param>
    /// <param name="data">The object to be serialized to JSON format.</param>
    /// <remarks>
    /// Utilizes the <see cref="JsonSerializer"/> with indented formatting for readability.
    /// Leverages the <see cref="FileUtil.WriteTextFile"/> method to handle the actual file writing operation.
    /// </remarks>
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

    /// <summary>
    /// Reads a JSON file from the specified path and deserializes it into an object of the given type.
    /// This method utilizes the `System.Text.Json.JsonSerializer` for deserialization.
    /// </summary>
    /// <param name="returnType">The type of the object to deserialize the JSON data into.</param>
    /// <param name="path">The path of the JSON file to read and deserialize.</param>
    /// <returns>An object of the specified type containing the data deserialized from the JSON file.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified path is null or empty.</exception>
    /// <exception cref="System.IO.FileNotFoundException">Thrown when the file at the specified path does not exist.</exception>
    /// <exception cref="System.Text.Json.JsonException">Thrown when the JSON is invalid or cannot be deserialized into the specified type.</exception>
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

    /// <summary>
    /// Checks if a specific file exists within a given directory.
    /// </summary>
    /// <param name="path">The path to the directory to check.</param>
    /// <param name="file">The name of the file to look for within the directory.</param>
    /// <returns>Returns true if the file exists in the specified directory; otherwise, false.</returns>
    /// <remarks>
    /// This method utilizes the <see cref="File.Exists"/> method to determine the existence of the file.
    /// </remarks>
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