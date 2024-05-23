namespace VDHelper;

public class Config
{
    public List<ConfigEntry> ConfigEntries { get; set; } = new();

    public ConfigEntry? GetConfigEntry(string entryName)
    {
        return ConfigEntries.Find(ce => ce.Name == entryName);
    }
    public string VDLocation { get; set; } =
        "C:\\Program Files\\Virtual Desktop Streamer\\VirtualDesktop.Streamer.exe";
}