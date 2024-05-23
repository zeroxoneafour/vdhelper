using System.Security.Cryptography;

namespace VDHelper;

public class ConfigEntry(string name, string path, string exec, string args)
{
    public string Name => name;
    public string Path => path;
    public string Exec => exec;
    public string Args => args;

    public override string ToString() => $"{Name}: {Path}\\{Exec} {Args}";
}