using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts;

public enum ImageFormat {
    Png,
    Jpg,
    Bmp,
    Webp
}

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

    public static async Task<Image> FetchImage(string url) {
        string fileType = url.Split(".").LastOrDefault();

        switch (fileType) {
            case "png":
                return await FetchImage(url, ImageFormat.Png);
            case "jpg":
            case "jpeg":
                return await FetchImage(url, ImageFormat.Jpg);
            case "bmp":
                return await FetchImage(url, ImageFormat.Bmp);
            case "webp":
                return await FetchImage(url, ImageFormat.Webp);
            default:
                return null;
        }
    }

    public static async Task<Image> FetchImage(string url, ImageFormat imageFormat) {
        try {
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient()) {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode) {
                    Image image = new Image();
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();

                    switch (imageFormat) {
                        case ImageFormat.Png:
                            image.LoadPngFromBuffer(buffer);
                            break;
                        case ImageFormat.Jpg:
                            image.LoadJpgFromBuffer(buffer);
                            break;
                        case ImageFormat.Bmp:
                            image.LoadBmpFromBuffer(buffer);
                            break;
                        case ImageFormat.Webp:
                            image.LoadWebpFromBuffer(buffer);
                            break;
                    }

                    return image;
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