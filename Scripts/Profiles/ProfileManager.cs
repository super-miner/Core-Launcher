using System.Collections.Generic;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.Profiles; 

public static class ProfileManager {
    public static List<Profile> Profiles = new List<Profile>();

    public static void Init() {
        if (!FileUtil.DirectoryExists(GetProfilesPath())) {
            FileUtil.CreateDirectory(GetProfilesPath());
        }
        
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
    }
    
    public static Profile AddProfile(string id) {
        Profile profile = new Profile(id);
        
        Profiles.Add(profile);
        return profile;
    }
    
    public static Profile AddProfile(string name, bool server) {
        long currentTime = (long) (Time.GetUnixTimeFromSystem() * 1000.0);
        Profile profile = new Profile(currentTime.ToString(), name, server);
        
        Profiles.Add(profile);
        return profile;
    }

    public static bool ProfileTemplatesExist() {
        return FileUtil.DirectoryExists(GetProfileTemplatePath(true)) && FileUtil.DirectoryExists(GetProfileTemplatePath(false));
    }

    public static void CreateProfileTemplates() {
        CreateProfileTemplate(true);
        CreateProfileTemplate(false);
    }

    public static void CreateProfileTemplate(bool server) {
        string profileTemplatePath = GetProfileTemplatePath(server);

        if (!FileUtil.DirectoryExists(profileTemplatePath)) {
            FileUtil.CreateDirectory(profileTemplatePath);
        }

        string profileTemplateDataPath = $"{profileTemplatePath}CoreKeeperData/";
        FileUtil.CopyDirectory(GameManager.GetCoreKeeperDataPath(server), profileTemplateDataPath);
    }
    
    public static string GetProfilesPath() {
        return FileUtil.GetPath(PathType.Profiles);
    }
    
    public static string GetProfileTemplatePath(bool server) {
        return $"{FileUtil.GetPath(PathType.AppData)}Templates/{(server ? "ServerProfileTemplate" : "ProfileTemplate")}/";
    }

    private static void OnDeserializeStoredData() {
        foreach (string projectDirectoryPath in FileUtil.GetDirectories(GetProfilesPath())) {
            string projectDirectoryName = FileUtil.GetDirectoryName(projectDirectoryPath);

            Profile profile = AddProfile(projectDirectoryName);
            profile.Deserialize();
        }
    }

    private static void OnSerializeStoredData() {
        foreach (Profile profile in Profiles) {
            profile.Serialize();
        }
    }
}