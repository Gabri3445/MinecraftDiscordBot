namespace MinecraftDiscordBot;

public class Options
{
    public string Token { get; }
    public string Location { get; }
    public string Arguments { get; }
    public string WorkingDirectory { get; }

    public Options(string token, string location, string arguments, string workingDirectory)
    {
        Token = token;
        Location = location;
        Arguments = arguments;
        WorkingDirectory = workingDirectory;
    }
}
    