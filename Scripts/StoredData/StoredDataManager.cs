using System;
using System.Collections.Generic;
using Godot;

namespace CoreLauncher.Scripts.StoredData;

public static class StoredDataManager {
    public static StoredData Data = new StoredData();

    private static List<Action<StoredData>> _deserializeCallbacks = new List<Action<StoredData>>();

    public static void AddDeserializedCallback(Action<StoredData> callback) {
        _deserializeCallbacks.Add(callback);
    }
    
    public static void Serialize() {
        FileManager.WriteJSONFile(GetPath(), Data);
    }
    
    public static void Deserialize() {
        Data = FileManager.ReadJsonFile<StoredData>(GetPath());
        
        OnDerserialized(Data);
    }
    
    public static string GetPath() {
        return FileManager.GetPath(PathType.AppData) + "Settings.json";
    }
    
    private static void OnDerserialized(StoredData data) {
        foreach (Action<StoredData> callback in _deserializeCallbacks) {
            callback.Invoke(data);
        }
    }
}