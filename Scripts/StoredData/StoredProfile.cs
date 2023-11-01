using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.StoredData; 

public class StoredProfile {
    public string Name { get; set; } = "";

    public StoredProfile(string name) {
        this.Name = name;
    }
}