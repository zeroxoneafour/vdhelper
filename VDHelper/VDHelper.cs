using Microsoft.Win32;

namespace VDHelper;

public class VDHelper
{
    private string _configLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "vdhelper.json");

    private Config _cfg;
    private readonly string vdLocation;

    public VDHelper()
    {
        string? vdPath =
            Registry.LocalMachine.OpenSubKey("SOFTWARE\\Virtual Desktop, Inc.\\Virtual Desktop Streamer")?
                .GetValue("Path") as string;
        if (vdPath is null)
        {
            throw new Exception("Virtual Desktop not found");
        }
        vdLocation = $"{vdPath}\\VirtualDesktop.Streamer.exe";
        if (File.Exists(_configLocation))
        {
            string configString = File.ReadAllText(_configLocation);
            _cfg = Config.Deserialize(configString);
        }
        else
        {
            _cfg = new Config();
        }
    }
    
    public void LaunchGame(string name)
    {
        var game = _cfg.GetGame(name);

        if (game is null)
        {
            Console.WriteLine($"Game {name} not found");
            return;
        }

        game.Launch(vdLocation);
    }

    public void AddNormalGame(string name, string path, string exec, string args)
    {
        if (_cfg.GetGame(name) is not null)
        {
            Console.WriteLine("Entry already exists");
            return;
        }
        NormalGame entry = new(name, path, exec, args);
        _cfg.Games.Add(entry);
        Console.WriteLine($"Added {name}");
        WriteConfig();
    }

    public void AddSteamGame(string name, string appId, string args)
    {
        if (_cfg.GetGame(name) is not null)
        {
            Console.WriteLine("Entry already exists");
            return;
        }

        SteamGame entry = new(name, appId, args);
        _cfg.Games.Add(entry);
        Console.WriteLine($"Added {name}");
        WriteConfig();
    }

    public void RemoveGame(string name)
    {
        _cfg.Games = _cfg.Games.FindAll(x => x.Name != name);
        Console.WriteLine($"Removed {name}");
        WriteConfig();
    }
    
    public void PrintGameInfo(string name)
    {
        var game = _cfg.GetGame(name);
        Console.WriteLine(game?.ToString() ?? $"Could not find game {name}");
    }

    public void ListGames()
    {
        Console.WriteLine(String.Join(", ", _cfg.Games.Select(ce => ce.Name)));
    }

    private void WriteConfig()
    {
        string jsonString = _cfg.Serialize();
        File.WriteAllText(_configLocation, jsonString);
    }
}