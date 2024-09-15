using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.ModIO.LocalInfo;
using CoreLauncher.Scripts.Profiles.JSON;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.Profiles; 

public class Profile {
    public string Id;
    public string Name;
    public bool Server;

    private List<int> _installedMods = new List<int>();
    private List<int> _queuedMods = new List<int>();

    public Profile(string id) {
        Id = id;
    }
    
    public Profile(string id, string name, bool server) {
        Id = id;
        Name = name;
        Server = server;

        ulong startTime = Time.GetTicksMsec();
        FileUtil.CopyDirectory(ProfileManager.GetProfileTemplatePath(Server), GetPath());
        GD.Print($"Time: {Time.GetTicksMsec() - startTime}");
    }

    public void Deserialize() {
        ProfileInfo profileInfo = FileUtil.ReadJsonFile<ProfileInfo>(GetConfigsPath());
        
        Name = profileInfo.Name;
        Server = profileInfo.Server;
        _queuedMods = profileInfo.QueuedMods;
        
        _installedMods = new List<int>();
        if (FileUtil.DirectoryExists(GetModsPath())) {
            foreach (string modDirectoryPath in FileUtil.GetDirectories(GetModsPath())) {
                LocalModInfo localModInfo = LocalModInfo.FromString(modDirectoryPath);

                if (localModInfo == null) {
                    continue;
                }

                _installedMods.Add(localModInfo.Id);
            }
        }
    }

    public void Serialize() {
        ProfileInfo profileInfo = new ProfileInfo() {
            Name = Name,
            Server = Server,
            QueuedMods = _queuedMods
        };
        
        FileUtil.WriteJSONFile(GetConfigsPath(), profileInfo);
    }

    public async Task Install() {
        ProfileManager.GetLastLoadedProfile()?.Uninstall();
        ProfileManager.LastLoadedProfileId = Id;
        
        await ManageQueuedMods();

        string toPath = GameManager.GetCoreKeeperDataPath(Server);
        if (FileUtil.DirectoryExists(toPath)) {
            FileUtil.DeleteDirectory(toPath);
        }
        FileUtil.CopyDirectory(GetCoreKeeperDataPath(), toPath);

        toPath = GameManager.GetAppDataPath();
        if (FileUtil.DirectoryExists(toPath)) {
            FileUtil.DeleteDirectory(toPath);
        }
        FileUtil.CopyDirectory(GetAppDataPath(), toPath);
    }

    public void Uninstall() {
        if (!FileUtil.DirectoryExists(GetPath())) {
            FileUtil.CreateDirectory(GetPath());
        }
        
        string toPath = GetCoreKeeperDataPath();
        if (FileUtil.DirectoryExists(toPath)) {
            FileUtil.DeleteDirectory(toPath);
        }
        FileUtil.CopyDirectory(GameManager.GetCoreKeeperDataPath(Server), toPath);
        
        toPath = GetAppDataPath();
        if (FileUtil.DirectoryExists(toPath)) {
            FileUtil.DeleteDirectory(toPath);
        }
        FileUtil.CopyDirectory(GameManager.GetAppDataPath(), toPath);
    }

    public void Delete() {
        FileUtil.DeleteDirectory(GetPath());
    }

    public void QueueAddMod(int modId) {
        _queuedMods.Add(modId);
    }
    
    public void QueueRemoveMod(int modId) {
        _queuedMods.Remove(modId);
    }

    public async Task ManageQueuedMods() {
        string modsPath = GetModsPath();

        if (!FileUtil.DirectoryExists(modsPath)) {
            FileUtil.CreateDirectory(modsPath);
        }
        
        foreach (string modDirectoryPath in FileUtil.GetDirectories(modsPath)) {
            LocalModInfo localModInfo = LocalModInfo.FromString(modDirectoryPath);

            if (localModInfo == null) {
                continue;
            }
            
            ModInfo modInfo = ModManager.GetModInfo(localModInfo.Id);

            if (localModInfo.Version != modInfo.ModFile.Version) {
                GD.Print($"Profile: The mod {localModInfo.Name} is out of date updating ({localModInfo.Version}) -> ({modInfo.ModFile.Version})...");
                
                FileUtil.DeleteDirectory(modDirectoryPath);
                _installedMods.Remove(localModInfo.Id);
            }
        }
        
        List<int> queuedModsWithDependencies = await ModManager.GetDependencies(_queuedMods);

        (List<int> addedMods, List<int> removedMods) = ModManager.GetModDeltas(_installedMods, queuedModsWithDependencies);

        foreach (int modId in addedMods) {
            await ModManager.DownloadMod(modId, GetModsPath());

            LocalModInfo localModInfo = ModManager.GetLocalModInfo(modId, GetModsPath());
            if (!ModManager.IsModSafe(localModInfo)) {
                GD.Print($"Profile: The mod {localModInfo.Name} appears to be elevated access but is not marked as such, removing it..."); //TODO: Put a pop up here to determine whether the user wants to continue with the installation of this mod.
                
                FileUtil.DeleteDirectory(localModInfo.Path);
            }
        }

        foreach (int modId in removedMods) {
            ModManager.RemoveMod(modId, GetModsPath());
        }

        _installedMods = new List<int>(queuedModsWithDependencies);
    }

    public List<int> GetAddedMods() {
        return _queuedMods;
    }

    public string GetPath() {
        return $"{ProfileManager.GetProfilesPath()}{Id}/";
    }
    
    public string GetConfigsPath() {
        return $"{ProfileManager.GetProfilesPath()}{Id}/Config.json";
    }

    public string GetCoreKeeperDataPath() {
        return $"{GetPath()}CoreKeeperData/";
    }
    
    public string GetAppDataPath() {
        return $"{GetPath()}AppData/";
    }
    
    public string GetModsPath() {
        return $"{GetCoreKeeperDataPath()}StreamingAssets/Mods/";
    }

    public string GetModConfigsPath() {
        return $"{GetPath()}ModConfigs/";
    }
}