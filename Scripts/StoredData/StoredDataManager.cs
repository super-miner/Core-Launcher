using System;
using System.Collections.Generic;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.StoredData; 

public delegate void OnDeserializeStoredData();
public delegate void OnSerializeStoredData();
public delegate void OnStoredDataDeserialized();

public static class StoredDataManager {
    public static event OnDeserializeStoredData DeserializeStoredDataEvent;
    public static event OnSerializeStoredData SerializeStoredDataEvent;
    public static event OnStoredDataDeserialized StoredDataDeserializedEvent;

    public static bool HasDeserialized = false;
    
    private static readonly string StoredDataFolder = "Data/";
    private static Dictionary<Type, StoredDataGroup> _groups = null;

    public static void Deserialize() {
        if (_groups == null) {
            _groups = new Dictionary<Type, StoredDataGroup>();
            
            PopulateGroupsList();
        }

        foreach (Type type in _groups.Keys) {
            string path = GetPath() + _groups[type].GetPath();

            if (FileUtil.FileExists(path)) {
                _groups[type] = (StoredDataGroup) FileUtil.ReadJsonFile(type, path);
            }
            else {
                _groups[type] = (StoredDataGroup) Activator.CreateInstance(type);
            }
        }

        HasDeserialized = true;
        
        DeserializeStoredDataEvent?.Invoke();
        StoredDataDeserializedEvent?.Invoke();

        if (DeserializeStoredDataEvent != null) {
            foreach (Delegate _delegate in DeserializeStoredDataEvent.GetInvocationList()) {
                GD.Print($"Stored Data Manager: Called deserialization function {_delegate.Method.DeclaringType}.{_delegate.Method.Name}(...)");
            }
        }
    }

    public static void Serialize() {
        SerializeStoredDataEvent?.Invoke();

        if (DeserializeStoredDataEvent != null) {
            foreach (Delegate _delegate in SerializeStoredDataEvent.GetInvocationList()) {
                GD.Print($"Stored Data Manager: Called serialization function {_delegate.Method.DeclaringType}.{_delegate.Method.Name}(...)");
            }
        }

        foreach (StoredDataGroup group in _groups.Values) {
            string path = GetPath() + group.GetPath();
            
            FileUtil.WriteJSONFile(path, group);
        }
    }

    public static T GetStoredDataGroup<T>() where T : StoredDataGroup {
        return (T) _groups[typeof(T)];
    }
    
    public static string GetPath() {
        return FileUtil.GetPath(PathType.AppData) + StoredDataFolder;
    }

    private static void PopulateGroupsList() {
        foreach (Type type in ReflectionUtil.GetDerivedTypes<StoredDataGroup>()) {
            object storedDataGroupObject = Activator.CreateInstance(type);
            StoredDataGroup storedDataGroup = (StoredDataGroup) Convert.ChangeType(storedDataGroupObject, type);
            
            _groups.Add(type, storedDataGroup);
        }
    }
}