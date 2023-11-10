using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;

namespace CoreLauncher.Scripts; 

public static class ModIOManager {
    public static string UserID = "";
    public static string ApiKey = "";

    public static void Init() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
    }
    
    private static void OnDeserializeStoredData() {
        UserID = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOUserID;
        ApiKey = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey;
    }
    
    private static void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOUserID = UserID;
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey = ApiKey;
    }
}