namespace VDHelper;

public interface ILaunchableGame
{
    public string Name { get; }
    public void Launch(string vdLocation);
}