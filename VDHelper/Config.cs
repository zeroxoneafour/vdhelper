using System.Text.Json;
using System.Text.Json.Serialization;

namespace VDHelper;

[Serializable]
public class Config
{
    [JsonIgnore]
    public List<ILaunchableGame> Games { get; set; }

    [JsonInclude]
    private List<SteamGame> _steamGames;
    [JsonInclude]
    private List<NormalGame> _normalGames;

    public Config()
    {
        _steamGames = new();
        _normalGames = new();
        Games = new List<ILaunchableGame>();
    }

    public static Config Deserialize(string jsonString)
    {
        var config = JsonSerializer.Deserialize<Config>(jsonString);
        if (config is null)
        {
            throw new Exception("Could not deserialize config");
        }

        config.Games = new List<ILaunchableGame>().Concat(config._normalGames).Concat(config._steamGames).ToList();
        return config;
    }

    public string Serialize()
    {
        _normalGames = new();
        _steamGames = new();
        foreach (var game in Games)
        {
            switch (game)
            {
                case NormalGame normalGame:
                    _normalGames.Add(normalGame);
                    break;
                case SteamGame steamGame:
                    _steamGames.Add(steamGame);
                    break;
            }
        }
        return JsonSerializer.Serialize(this);
    }

    public ILaunchableGame? GetGame(string name)
    {
        return Games.Find(ce => ce.Name == name);
    }
}