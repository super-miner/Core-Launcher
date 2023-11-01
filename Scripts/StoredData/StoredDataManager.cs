using System.Collections.Generic;

namespace CoreLauncher.Scripts.StoredData;

public static class StoredDataManager {
    public static StoredData data = new StoredData();

    public static void Serialize() {
        FileManager.WriteJSONFile(FileManager.GetPath(PathType.StoredData), data);
    }
    
    public static string GetPath() {
        return FileManager.GetPath(PathType.StoredData);
    }
}