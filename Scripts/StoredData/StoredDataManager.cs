using System;
using System.Collections.Generic;
using Godot;

namespace CoreLauncher.Scripts.StoredData;

public delegate void OnStoredDataDeserialized();

public static class StoredDataManager {
    public static StoredData Data = new StoredData();
    public static event OnStoredDataDeserialized StoredDataDeserializedEvent;
    
    public static void Serialize() {
        FileManager.WriteJSONFile(GetPath(), Data);
    }
    
    public static void Deserialize() {
        Data = FileManager.ReadJsonFile<StoredData>(GetPath());
        
        StoredDataDeserializedEvent?.Invoke();
    }
    
    public static string GetPath() {
        return FileManager.GetPath(PathType.AppData) + "Settings.json";
    }
}