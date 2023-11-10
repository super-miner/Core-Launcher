using System;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.StoredData; 

public delegate void OnDeserializeStoredData();
public delegate void OnSerializeStoredData();
public delegate void OnStoredDataDeserialized();

public static class StoredDataManager {
    public static event OnDeserializeStoredData DeserializeStoredDataEvent;
    public static event OnSerializeStoredData SerializeStoredDataEvent;
    public static event OnStoredDataDeserialized StoredDataDeserializedEvent;
    
    private static readonly string StoredDataFolder = "Data/";
    private static Dictionary<Type, StoredDataGroup> _groups = null;

    public static void Deserialize() {
        if (_groups == null) {
            _groups = new Dictionary<Type, StoredDataGroup>();
            
            PopulateGroupsList();
        }

        foreach (Type type in _groups.Keys) {
            string path = GetPath() + _groups[type].GetPath();

            if (FileManager.FileExists(path)) {
                _groups[type] = (StoredDataGroup) FileManager.ReadJsonFile(type, path);
            }
            else {
                _groups[type] = (StoredDataGroup) Activator.CreateInstance(type);
            }
        }
        
        DeserializeStoredDataEvent?.Invoke();
        StoredDataDeserializedEvent?.Invoke();
    }

    public static void Serialize() {
        SerializeStoredDataEvent?.Invoke();
        
        foreach (StoredDataGroup group in _groups.Values) {
            string path = GetPath() + group.GetPath();
            
            FileManager.WriteJSONFile(path, group);
        }
    }

    public static T GetStoredDataGroup<T>() where T : StoredDataGroup {
        return (T) _groups[typeof(T)];
    }
    
    public static string GetPath() {
        return FileManager.GetPath(PathType.AppData) + StoredDataFolder;
    }

    private static void PopulateGroupsList() {
        foreach (Type type in ReflectionUtil.GetDerivedTypes<StoredDataGroup>()) {
            object storedDataGroupObject = Activator.CreateInstance(type);
            StoredDataGroup storedDataGroup = (StoredDataGroup) Convert.ChangeType(storedDataGroupObject, type);
            
            _groups.Add(type, storedDataGroup);
        }
    }
}