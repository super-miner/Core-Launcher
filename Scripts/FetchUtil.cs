using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts; 

public static class FetchUtil {
    public static async Task<string> Fetch(string url) {
        try {
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient()) {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        catch(WebException exception) {
            GD.PrintErr($"Error fetching url {url}.");
            GD.PrintErr(exception.Message);
        }

        return null;
    }
}