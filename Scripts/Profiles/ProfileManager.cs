using System.Collections.Generic;
using System.Linq;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.Profiles; 

public static class ProfileManager {
    public static string LastLoadedProfileId = "";
    public static List<Profile> Profiles = new List<Profile>();

    public static void Init() {
        if (!FileUtil.DirectoryExists(GetProfilesPath())) {
            FileUtil.CreateDirectory(GetProfilesPath());
        }
        
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;

        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }
    
    public static Profile AddProfile(string id) {
        Profile profile = new Profile(id);
        
        Profiles.Add(profile);
        return profile;
    }
    
    public static Profile CreateAndAddProfile(string name, bool server) {
        long currentTime = (long) (Time.GetUnixTimeFromSystem() * 1000.0);
        Profile profile = new Profile(currentTime.ToString(), name, server);
        
        Profiles.Add(profile);
        return profile;
    }

    public static void DeleteProfile(Profile profile) {
        profile.Delete();
        Profiles.Remove(profile);
    }

    public static bool ProfileTemplatesExist() {
        return FileUtil.DirectoryExists(GetProfileTemplatePath(true)) && FileUtil.DirectoryExists(GetProfileTemplatePath(false));
    }

    public static void CreateProfileTemplates() {
        CreateProfileTemplate(true);
        CreateProfileTemplate(false);
    }

    public static void CreateProfileTemplate(bool server) {
        CreateBackup(server);
        
        string profileTemplatePath = GetProfileTemplatePath(server);

        if (!FileUtil.DirectoryExists(profileTemplatePath)) {
            FileUtil.CreateDirectory(profileTemplatePath);
        }

        string profileTemplateDataPath = $"{profileTemplatePath}CoreKeeperData/";
        FileUtil.CopyDirectory(GameManager.GetCoreKeeperDataPath(server), profileTemplateDataPath);
        
        string profileTemplateAppDataPath = $"{profileTemplatePath}AppData/";
        FileUtil.CopyDirectory(GameManager.GetAppDataPath(), profileTemplateAppDataPath);
        
        string profileTemplateModsPath = $"{profileTemplatePath}CoreKeeperData/StreamingAssets/Mods/";
        foreach (string directoryPath in FileUtil.GetDirectories(profileTemplateModsPath)) {
            FileUtil.DeleteDirectory(directoryPath);
        }
    }

    public static void CreateBackup(bool server, bool backupDataOnly = false) {
        string backupPath = GetBackupPath(server);

        if (!FileUtil.DirectoryExists(backupPath)) {
            FileUtil.CreateDirectory(backupPath);
        }
        
        string backupDataPath = $"{backupPath}CoreKeeperData/";
        if (!FileUtil.DirectoryExists(backupDataPath)) {
            FileUtil.CreateDirectory(backupDataPath);
        }
        FileUtil.CopyDirectory(GameManager.GetCoreKeeperDataPath(server), backupDataPath);
        
        string backupAppDataPath = $"{backupPath}AppData/";
        if (!FileUtil.DirectoryExists(backupAppDataPath)) {
            FileUtil.CreateDirectory(backupAppDataPath);
        }
        FileUtil.CopyDirectory(GameManager.GetAppDataPath(), backupAppDataPath);
    }
    
    public static string GetProfilesPath() {
        return FileUtil.GetPath(PathType.Profiles);
    }
    
    public static string GetProfileTemplatePath(bool server) {
        return $"{FileUtil.GetPath(PathType.AppData)}Templates/{(server ? "ServerProfileTemplate" : "ProfileTemplate")}/";
    }
    
    public static string GetBackupPath(bool server) {
        long currentTime = (long) (Time.GetUnixTimeFromSystem() * 1000.0);
        
        return $"{FileUtil.GetPath(PathType.AppData)}Backups/{(server ? "ServerBackups" : "ClientBackups")}/{currentTime}/";
    }

    public static Profile GetLastLoadedProfile() {
        return Profiles.FirstOrDefault(profile => profile.Id == LastLoadedProfileId);
    }

    private static void OnDeserializeStoredData() {
        foreach (string projectDirectoryPath in FileUtil.GetDirectories(GetProfilesPath())) {
            string projectDirectoryName = FileUtil.GetDirectoryName(projectDirectoryPath);

            Profile profile = AddProfile(projectDirectoryName);
            profile.Deserialize();
        }

        LastLoadedProfileId = StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().LastLoadedIntId;
    }

    private static void OnSerializeStoredData() {
        foreach (Profile profile in Profiles) {
            profile.Serialize();
        }
        
        StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().LastLoadedIntId = LastLoadedProfileId;
    }
}