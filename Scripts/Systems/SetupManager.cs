using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;

namespace CoreLauncher.Scripts.Systems; 

public static class SetupManager {
    public static bool SetupComplete = false;

    public static void Init() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;

        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }
    
    private static void OnDeserializeStoredData() {
        SetupComplete = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SetupComplete;
    }

    private static void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().SetupComplete = SetupComplete;
    }
}