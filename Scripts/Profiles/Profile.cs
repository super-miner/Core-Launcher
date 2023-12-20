using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLauncher.Scripts.ModIO;
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
        QueueOutdatedMods();
        await ManageQueuedMods();
        
        FileUtil.CopyDirectory(GetCoreKeeperDataPath(), GameManager.GetCoreKeeperDataPath(Server));
    }

    public void Uninstall() {
        FileUtil.CopyDirectory(GameManager.GetCoreKeeperDataPath(Server), GetCoreKeeperDataPath());
    }

    public void QueueAddMod(int modId) {
        _queuedMods.Add(modId);
    }
    
    public void QueueRemoveMod(int modId) {
        _queuedMods.Remove(modId);
    }

    public void QueueOutdatedMods() {
        if (!FileUtil.DirectoryExists(GetModsPath())) {
            FileUtil.CreateDirectory(GetModsPath());
        }
        
        foreach (string modDirectoryPath in FileUtil.GetDirectories(GetModsPath())) {
            LocalModInfo localModInfo = LocalModInfo.FromString(modDirectoryPath);

            if (ModManager.IsModOutdated(localModInfo)) {
                FileUtil.DeleteDirectory(modDirectoryPath);
                QueueAddMod(localModInfo.Id);
            }
        }
    }

    public async Task ManageQueuedMods() {
        List<int> queuedModsWithDependencies = await ModManager.GetDependencies(_queuedMods);

        (List<int> addedMods, List<int> removedMods) modDeltas = ModManager.GetModDeltas(_installedMods, queuedModsWithDependencies);
        
        foreach (int modId in queuedModsWithDependencies) {
            await ModManager.DownloadMod(modId, GetModsPath());

            LocalModInfo localModInfo = ModManager.GetLocalModInfo(modId, GetModsPath());
            if (!ModManager.IsModSafe(localModInfo)) {
                GD.Print($"Profile: The mod {localModInfo.Name} appears to be elevated access but is not marked as such, removing it..."); //TODO: Put a pop up here to determine whether the user wants to continue with the installation of this mod.
                
                FileUtil.DeleteDirectory(localModInfo.Path);

                continue;
            }
        }

        _installedMods = new List<int>(_queuedMods);
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
    
    public string GetModsPath() {
        return $"{GetCoreKeeperDataPath()}StreamingAssets/Mods/";
    }

    public string GetModConfigsPath() {
        return $"{GetPath()}ModConfigs/";
    }
}