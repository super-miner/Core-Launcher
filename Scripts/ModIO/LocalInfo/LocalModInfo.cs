using System.Text.RegularExpressions;
using CoreLauncher.Scripts.Systems;

namespace CoreLauncher.Scripts.ModIO.LocalInfo; 

public class LocalModInfo {
    private static readonly Regex LocalModsRegex = new Regex(@"CL_Mod_(\d+)_(.+)_+?(.*)");

    public string Path;
    public string Name;
    public int Id;
    public string Version;

    public LocalModInfo() {
        
    }

    public LocalModInfo(string path, string name, int id, string version) {
        Path = path;
        Name = name;
        Id = id;
        Version = version;
    }
    
    public static LocalModInfo FromString(string modDirectoryPath) {
        string modDirectoryName = FileUtil.GetDirectoryName(modDirectoryPath);
        Match match = LocalModsRegex.Match(modDirectoryName);

        if (!match.Success) {
            return null;
        }

        LocalModInfo localModInfo = new LocalModInfo();
        
        string modIdString = match.Groups[1].ToString();
        if (int.TryParse(modIdString, out int modId)) {
            localModInfo.Id = modId;
        }
        else {
            return null;
        }

        localModInfo.Path = modDirectoryPath;
        localModInfo.Name = match.Groups[2].ToString();
        localModInfo.Version = match.Groups[3].ToString();

        return localModInfo;
    }

    public string ToString(LocalModInfo localModInfo) {
        return ModManager.GetModLocalDirectoryName(Id, Name, Version);
    }
}