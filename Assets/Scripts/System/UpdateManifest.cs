[System.Serializable]
public class UpdateManifest
{
    public string latestVersion;
    public PlatformInfo windows;
}

[System.Serializable]
public class PlatformInfo
{
    public string url;
    public string sha256;
    public long sizeBytes;
}
