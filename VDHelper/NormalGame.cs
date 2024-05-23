using System;
using System.Diagnostics;

namespace VDHelper;

public class NormalGame(string name, string path, string exec, string args) : ILaunchableGame
{
    public string Name => name;
    public string Path => path;
    public string Exec => exec;
    public string Args => args;

    public override string ToString() => $"{Name}: {Path}\\{Exec} {Args}";

    public void Launch(string vdLocation)
    {
        ProcessStartInfo startInfo = new();
        startInfo.WorkingDirectory = Path;
        startInfo.Arguments = $"\"{Exec}\" {Args}";
        startInfo.FileName = vdLocation;
        Console.WriteLine($"Launching {name}");
        Process.Start(startInfo);
    }
}