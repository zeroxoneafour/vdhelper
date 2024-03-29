﻿using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace VDHelper;

public class VDHelper
{
    private string[] _args;
    private string _configLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "vdhelper.json");

    private Config _cfg;

    public VDHelper(string[] args)
    {
        _args = args;
        if (File.Exists(_configLocation))
        {
            string configString = File.ReadAllText(_configLocation);
            _cfg = JsonSerializer.Deserialize<Config>(configString)!;
        }
        else
        {
            _cfg = new Config();
        }
    }

    public void Run()
    {
        if (_args.Length == 0)
        {
            ParseError();
            return;
        }
        
        switch (_args[0])
        {
            case "launch":
                if (_args.Length != 2)
                {
                    ParseError();
                    return;
                }
                LaunchGame(_args[1]);
                break;
            case "add":
                switch (_args.Length)
                {
                    case 4:
                        AddGame(_args[1], _args[2], _args[3], "");
                        break;
                    case 5:
                        AddGame(_args[1], _args[2], _args[3], _args[4]);
                        break;
                    default:
                        ParseError();
                        break;
                }
                break;
            case "remove":
                if (_args.Length != 2)
                {
                    ParseError();
                    return;
                }
                RemoveGame(_args[1]);
                break;
            case "vdpath":
                if (_args.Length != 2)
                {
                    ParseError();
                    return;
                }
                SetVDPath(_args[1]);
                break;
            default:
                ParseError();
                break;
        }
    }

    private void LaunchGame(string name)
    {
        var games = _cfg.ConfigEntries;
        ConfigEntry? game = null;
        foreach (var g in games)
        {
            if (g.Name == name)
            {
                game = g;
                break;
            }
        }

        if (game is null)
        {
            Console.WriteLine($"Game {name} not found");
            return;
        }
        
        ProcessStartInfo startInfo = new();
        startInfo.WorkingDirectory = game.Path;
        startInfo.Arguments = $"\"{game.Exec}\" {game.Args}";
        startInfo.FileName = _cfg.VDLocation;
        Console.WriteLine($"Launching {name}");
        Process.Start(startInfo);
    }

    private void AddGame(string name, string path, string exec, string args)
    {
        var entries = _cfg.ConfigEntries.Find(x => x.Name == name);
        if (entries != null)
        {
            Console.WriteLine("Entry already exists");
            return;
        }
        ConfigEntry entry = new(name, path, exec, args);
        _cfg.ConfigEntries.Add(entry);
        Console.WriteLine($"Added {name}");
        WriteConfig();
    }

    private void RemoveGame(string name)
    {
        _cfg.ConfigEntries = _cfg.ConfigEntries.FindAll(x => x.Name != name);
        Console.WriteLine($"Removed {name}");
        WriteConfig();
    }

    private void SetVDPath(string path)
    {
        _cfg.VDLocation = path;
        WriteConfig();
    }
    private void ParseError()
    {
        Console.WriteLine("Failure parsing arguments, try to rtfm next time");
    }

    private void WriteConfig()
    {
        string jsonString = JsonSerializer.Serialize(_cfg);
        File.WriteAllText(_configLocation, jsonString);
    }
}