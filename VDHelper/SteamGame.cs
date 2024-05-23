using System.Diagnostics;
using Microsoft.Win32;

namespace VDHelper;

public class SteamGame(string name, string appId, string args) : ILaunchableGame
{
    public string Name => name;
    public string AppId => appId;
    public string Args => args;

    public void Launch(string vdLocation)
    {
        string? steamPath =
            Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam")?.GetValue("InstallPath") as string;
        if (steamPath is null)
        {
            Console.WriteLine("Steam path not found");
            return;
        }
        ProcessStartInfo startInfo = new();
        startInfo.Arguments = $"\"{steamPath}\\steam.exe\" -applaunch {AppId} {Args}";
        startInfo.FileName = vdLocation;
        Console.WriteLine($"Launching {name}");
        Process.Start(startInfo);
    }

    public override string ToString()
    {
        return $"{Name}: Steam AppId {AppId}, args {Args}";
    }
}