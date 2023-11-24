using System.Text.RegularExpressions;
using CoreLauncher.Scripts.ModIO.JsonStructures;

namespace CoreLauncher.Scripts.ModIO.LocalInfo; 

public class LocalModInfo {
    private static readonly Regex LocalModsRegex = new Regex(@"CL_Mod_(\d+)_(.+)_+?(.*)");
    
    public string Name;
    public int Id;
    public string Version;

    public LocalModInfo() {
        
    }

    public LocalModInfo(string name, int id, string version) {
        Name = name;
        Id = id;
        Version = version;
    }
    
    public static LocalModInfo FromString(string modDirectory) {
        Match match = LocalModsRegex.Match(modDirectory);

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
        
        localModInfo.Name = match.Groups[2].ToString();
        localModInfo.Version = match.Groups[3].ToString();

        return localModInfo;
    }

    public string ToString(LocalModInfo localModInfo) {
        return ModManager.GetModLocalDirectoryName(Id, Name, Version);
    }

    public string GetLocalDirectoryPath() {
        return ModManager.GetModLocalDirectoryPath(Id, Name, Version);
    }
}