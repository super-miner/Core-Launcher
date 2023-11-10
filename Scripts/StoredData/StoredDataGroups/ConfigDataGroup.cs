namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class ConfigDataGroup : StoredDataGroup {
    public override string GetPath() {
        return "Config.json";
    }
}