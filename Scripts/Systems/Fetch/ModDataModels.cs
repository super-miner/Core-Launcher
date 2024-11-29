using System;
using System.Collections.Generic;

public class ModData
{
    public int Id { get; set; }
    public SubmittedBy SubmittedBy { get; set; }
    public long DateUpdated { get; set; }
    public string Name { get; set; }
    public string DescriptionPlaintext { get; set; }
    public string ProfileUrl { get; set; }
    public Logo Logo { get; set; }
    public ModFile ModFile { get; set; }
    public List<Tag> Tags { get; set; }
    public Stats Stats { get; set; }
}

public class SubmittedBy
{
    public string Username { get; set; }
}

public class Logo
{
    public string Original { get; set; }
}

public class ModFile
{
    public string Version { get; set; }
    public DownloadUrl DownloadUrl { get; set; }
}

public class DownloadUrl
{
    public string BinaryUrl { get; set; }

    public bool IsValidUrl()
    {
        Uri uriResult;
        bool isValid = Uri.TryCreate(BinaryUrl, UriKind.Absolute, out uriResult)
                       && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return isValid;
    }
}

public class Tag
{
    public string Name { get; set; }
}

public class Stats
{
    public string RatingsDisplayText { get; set; }
}