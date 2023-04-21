namespace MinecraftDiscordBot;

public class Options
{
    public string Token { get; }
    public string Location { get; }

    public Options(string token, string location)
    {
        Token = token;
        Location = location;
    }
}
    