using Discord;
using Discord.WebSocket;

public class Program
{
    public static Task Main(string[] args) 
    {
        
        new Program().MainAsync().GetAwaiter().GetResult();
        return Task.CompletedTask;
    }

    private DiscordSocketClient _client;
    
    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        
        var token = "MTAyMDAxNzcxNTQ2Njk5Nzg0Mg.G64Vgb.yhvDHC-HE3r2k05CzdhWg_pkRa27vCqC9jNnNs";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        
        await Task.Delay(-1);
    }

    private async Task Client_Ready()
    {
        var startCommand = new SlashCommandBuilder()
            .WithName("start")
            .WithDescription("Start the server");
        
        var stopCommand = new SlashCommandBuilder()
            .WithName("stop")
            .WithDescription("Stop the server");
        
        await _client.CreateGlobalApplicationCommandAsync(startCommand.Build());
        await _client.CreateGlobalApplicationCommandAsync(stopCommand.Build());
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "start":
            case "stop":
                await command.RespondAsync(command.Data.Name);
                break;
        }
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}