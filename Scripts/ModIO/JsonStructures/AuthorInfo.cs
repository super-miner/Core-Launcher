using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class AuthorInfo {
    [JsonInclude] [JsonPropertyName("username")] public string Username;
}