using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MinecraftDiscordBot;

public class Program
{
    private const string OptionsPath = @"S:/Github Projects/C#/MinecraftDiscordBot/MinecraftDiscordBot/options.json";

    public static Task Main(string[] args) 
    {
        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "-h":
                    Console.WriteLine(@"Usage:
-c: Creates a empty config file
-s: Starts the bot
-h: Prints the usage");
                    return Task.CompletedTask;
                case "-c" when !File.Exists(OptionsPath):
                {
                    var data = JsonConvert.SerializeObject(new Options("", "", "", ""), Formatting.Indented);
                    File.WriteAllText(OptionsPath, data);
                    Console.WriteLine("Config file generated");
                    return Task.CompletedTask;
                }
                case "-c":
                    Console.WriteLine("Config file already exists");
                    return Task.CompletedTask;
                case "-s":
                    try
                    {
                        var optionsData = File.ReadAllText(OptionsPath);
                        var options = JsonConvert.DeserializeObject<Options>(optionsData);

                        bool ValidateConfig(Options options)
                        {
                            return options.Token != "" && File.Exists(options.Location);
                        }
                        if (options == null /*|| ValidateConfig(options) */)
                        {
                            Console.WriteLine("Invalid or missing config file");
                            return Task.CompletedTask;
                        }
                        else
                        {
                            new Program().MainAsync(options).GetAwaiter().GetResult();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid or missing config file");
                        return Task.CompletedTask;
                    }
                    return Task.CompletedTask;
            }
        }

        Console.WriteLine(@"Usage:
-c: Creates a empty config file
-s: Starts the bot
-h: Prints the usage");
        return Task.CompletedTask;
    }

    private DiscordSocketClient _client;
    
    public async Task MainAsync(Options options)
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        
        var token = options.Token;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _path = options.Location;
        _arguments = options.Arguments;
        _workingDirectory = options.WorkingDirectory;
        
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

    private static Process? _process;

    private static string _path = "";

    private static string _arguments = "";

    private static string _workingDirectory = "";

    private static async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "start":
                if (_process == null)
                {
                    var procInfo = new ProcessStartInfo(_path, _arguments)
                    {
                        WorkingDirectory = _workingDirectory
                    };
                    _process = Process.Start(procInfo);
                    await command.RespondAsync("Server started");
                }
                else
                {
                    await command.RespondAsync("Server already started");
                }
                break;
            case "stop":
                if (_process != null)
                {
                    await command.RespondAsync("Server stopped");
                    _process.CloseMainWindow();
                    _process.Dispose();
                }
                else
                {
                    await command.RespondAsync("Server not started");
                }
                break;
        }
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}