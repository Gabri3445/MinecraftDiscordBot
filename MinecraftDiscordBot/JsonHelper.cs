using Newtonsoft.Json;

namespace MinecraftDiscordBot;

public class JsonHelper
{
    public static async Task SerializeClassJson<T>(string path, T content) where T : class
    {
        await WriteToFileAsync(path, JsonConvert.SerializeObject(content, Formatting.Indented));
    }

    public static async Task<T?> DeserializeClassJson<T>(string path) where T : class
    {
        var content = await ReadFromFileAsync(path);
        return JsonConvert.DeserializeObject<T>(content);
    }

    private static async Task WriteToFileAsync(string path, string content)
    {
        await using var sw = new StreamWriter(path);
        await sw.WriteLineAsync(content);
        sw.Close();
        await sw.DisposeAsync();
    }

    private static async Task<string> ReadFromFileAsync(string path)
    {
        using var sr = new StreamReader(path);
        var content = await sr.ReadToEndAsync();
        sr.Close();
        sr.Dispose();
        return content;
    }
}