using System.Collections.Generic;

namespace CoreLauncher.Scripts.StoredData; 

public class StoredData {
    public string SteamPath { get; set; } = "";
    public string ModIOAPIKey { get; set; } = "";
    public string ModIOUserID { get; set; } = "";
    public List<StoredProfile> Profiles { get; set; } = new List<StoredProfile>();
}