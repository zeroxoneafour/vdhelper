using System.CommandLine;

namespace VDHelper;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var vdHelper = new VDHelper();
        var rootCommand = new RootCommand();

        var addGameCommand = new Command("add", "Add a normal game");
        var addSteamGameCommand = new Command("addSteam", "Add a steam game");
        var launchCommand = new Command("launch", "Launch a game");
        var removeCommand = new Command("remove", "Remove a game");
        var listCommand = new Command("list", "List all installed games");
        var infoCommand = new Command("info", "Get info about a game");

        var gameName = new Argument<string>("name", "The name of the game");
        var gamePath = new Argument<string>("path", "The path to the install directory of the game");
        var gameExe = new Argument<string>("exec", "The name of the executable");
        var gameArgs = new Argument<string>("args", () => "", "The arguments to pass to the game");
        var gameAppId = new Argument<string>("appid", "The steam AppId of the game");

        addGameCommand.AddArgument(gameName);
        addGameCommand.AddArgument(gamePath);
        addGameCommand.AddArgument(gameExe);
        addGameCommand.AddArgument(gameArgs);
        addGameCommand.SetHandler((name, path, exec, arguments) => vdHelper.AddNormalGame(name, path, exec, arguments),
            gameName, gamePath, gameExe, gameArgs);
        
        addSteamGameCommand.AddArgument(gameName);
        addSteamGameCommand.AddArgument(gameAppId);
        addSteamGameCommand.AddArgument(gameArgs);
        addSteamGameCommand.SetHandler((name, appId, arguments) => vdHelper.AddSteamGame(name, appId, arguments),
            gameName, gameAppId, gameArgs);
        
        launchCommand.AddArgument(gameName);
        launchCommand.SetHandler((name) => vdHelper.LaunchGame(name), gameName);
        
        removeCommand.AddArgument(gameName);
        removeCommand.SetHandler((name) => vdHelper.RemoveGame(name), gameName);
        
        listCommand.SetHandler(() => vdHelper.ListGames());
        
        infoCommand.AddArgument(gameName);
        infoCommand.SetHandler((name) => vdHelper.PrintGameInfo(name), gameName);

        rootCommand.AddCommand(addGameCommand);
        rootCommand.AddCommand(addSteamGameCommand);
        rootCommand.AddCommand(launchCommand);
        rootCommand.AddCommand(removeCommand);
        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(infoCommand);

        return await rootCommand.InvokeAsync(args);
    }
}