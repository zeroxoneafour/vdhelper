namespace VDHelper;

public class Config
{
    public List<ConfigEntry> ConfigEntries { get; set; } = new();
    public string VDLocation { get; set; } =
        "C:\\Program Files\\Virtual Desktop Streamer\\VirtualDesktop.Streamer.exe";
}