using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Systems.Fetch;
using Godot;

namespace CoreLauncher.Scripts.Systems.Fetch;

public enum ImageFormat {
    Png,
    Jpg,
    Bmp,
    Webp
}

public static class FetchManager {
    public static long GlobalCooldownEndTime {
        get {
            _globalCooldownMutex.Lock();
            long globalCooldownEndTime = _globalCooldownEndTime;
            _globalCooldownMutex.Unlock();
            return globalCooldownEndTime;
        }
        set {
            _globalCooldownMutex.Lock();
            _globalCooldownEndTime = value;
            _globalCooldownMutex.Unlock();
        }
    }
    
    private static Mutex _ongoingFetchesMutex = new Mutex();
    private static List<FetchInfo> _ongoingFetches = new List<FetchInfo>();

    private static Mutex _globalCooldownMutex = new Mutex();
    private static long _globalCooldownEndTime = -1;

    public static FetchInfo GetOngoingFetch(string url) {
        _ongoingFetchesMutex.Lock();

        FetchInfo output = _ongoingFetches.FirstOrDefault(fetch => fetch.Url == url);

        _ongoingFetchesMutex.Unlock();

        return output;
    }
    
    public static FetchInfo CreateOutgoingFetch(string url) {
        FetchInfo fetch = new FetchInfo(url);
        return AddOutgoingFetch(fetch);
    }

    public static FetchInfo AddOutgoingFetch(FetchInfo fetch) {
        _ongoingFetches.Add(fetch);
        return fetch;
    }

    public static void RemoveOutgoingFetch(FetchInfo fetch) {
        _ongoingFetches.Remove(fetch);
    }

    public static long GetGlobalCooldownTimeLeft() {
        return GlobalCooldownEndTime - (long)Time.GetTicksMsec();
    }
    
    public static async Task<HttpResponseMessage> Fetch(string url) {
        FetchInfo fetch = GetOngoingFetch(url);

        if (fetch != null) {
            return await fetch.Fetch();
        }
        else {
            return await CreateOutgoingFetch(url).Fetch();
        }
    }
    
    public static async Task<string> FetchString(string url) {
        HttpResponseMessage response = await Fetch(url);
        
        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadAsStringAsync();
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
        HttpResponseMessage response = await Fetch(url);
        
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

        return null;
    }

    public static async Task DownloadFile(string url, string path) {
        HttpResponseMessage response = await Fetch(url);

        if (response.IsSuccessStatusCode) {
            Stream responseStream = await response.Content.ReadAsStreamAsync();

            FileInfo fileInfo = new FileInfo(path); // TODO: Make this use FileUtil functions
            DirectoryInfo fileDirectory = fileInfo.Directory;
            if (fileDirectory != null && !fileDirectory.Exists) {
                fileDirectory.Create();
            }
            
            using (FileStream fileStream = File.OpenWrite(path)) {
                await responseStream.CopyToAsync(fileStream);
            }
        }
    }
}