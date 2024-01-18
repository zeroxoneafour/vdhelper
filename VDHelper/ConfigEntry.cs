using System.Security.Cryptography;

namespace VDHelper;

public class ConfigEntry(string name, string path, string exec, string args)
{
    public string Name { get; set; } = name;
    public string Path { get; set; } = path;
    public string Exec { get; set; } = exec;
    public string Args { get; set; } = args;
}