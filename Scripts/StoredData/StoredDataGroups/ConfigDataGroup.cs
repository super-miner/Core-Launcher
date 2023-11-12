namespace CoreLauncher.Scripts.StoredData.StoredDataGroups; 

public class ConfigDataGroup : StoredDataGroup {
    public bool ShowLibraryMods = false;
    
    public override string GetPath() {
        return "Config.json";
    }
}