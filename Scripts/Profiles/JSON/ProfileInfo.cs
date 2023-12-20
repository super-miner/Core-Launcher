using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.Profiles.JSON; 

public class ProfileInfo {
    [JsonInclude] public bool Server = false;
    [JsonInclude] public string Name = "";
    [JsonInclude] public List<int> QueuedMods = new List<int>();
}