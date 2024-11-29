using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public static class SteamLibraryParser
{
    public static Dictionary<string, List<string>> ReadSteamLibraryFile(string filePath)
    {
        // Ensure the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Steam library file not found", filePath);
        }

        var libraries = new Dictionary<string, List<string>>();

        // Regular expressions to match different parts of the structure
        var pathPattern = new Regex(@"""path""\s+""(?<path>[^""]+)""", RegexOptions.Compiled);
        var appPattern = new Regex(@"""(?<appId>\d+)""\s+""[^""]*""", RegexOptions.Compiled);

        string currentLibraryPath = null;
        List<string> currentLibraryApps = null;

        foreach (var line in File.ReadLines(filePath))
        {
            var pathMatch = pathPattern.Match(line);
            if (pathMatch.Success)
            {
                // If we have information about the previous library, we store it first
                if (currentLibraryPath != null && currentLibraryApps != null)
                {
                    libraries[currentLibraryPath] = new List<string>(currentLibraryApps);
                }

                // Start new library data
                currentLibraryPath = pathMatch.Groups["path"].Value;
                currentLibraryApps = new List<string>();
                continue;
            }

            var appMatch = appPattern.Match(line);
            if (appMatch.Success)
            {
                currentLibraryApps?.Add(appMatch.Groups["appId"].Value);
            }
        }

        // Add the last library to the result
        if (currentLibraryPath != null && currentLibraryApps != null)
        {
            libraries[currentLibraryPath] = currentLibraryApps;
        }

        return libraries;
    }
}