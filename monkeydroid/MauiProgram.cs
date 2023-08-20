
using monkeydroid.Models;
using monkeydroid.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace monkeydroid;

public static class MauiProgram
{
	internal static ServerCache Cache = new();
    internal static string ServerId = string.Empty;

	public static MauiApp CreateMauiApp()
	{
        //if (File.Exists(ServerCache.Pathname())) File.Delete(ServerCache.Pathname());

        LoadCache();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("NerdFonts-BitstromWeraSansMono.ttf", "MonoGlyph");
            });

		return builder.Build();
	}

    internal static void LoadCache()
    {
		Debug.WriteLine($"MauiApp.LoadCache");
        if (File.Exists(ServerCache.Pathname()))
        {
            Cache = JsonSerializer.Deserialize<ServerCache>(File.ReadAllText(ServerCache.Pathname()));
        }
        Debug.WriteLine($"...loaded {Cache.Servers.Count} servers");
		foreach (var s in Cache.Servers) Debug.WriteLine($"...{s.Id} {s.Hostname}:{s.Port}");
    }

    internal static void SaveCache()
    {
        Debug.WriteLine($"MauiApp.SaveCache");
        Cache.SavedTimestamp = DateTime.Now;
        File.WriteAllText(ServerCache.Pathname(), JsonSerializer.Serialize(Cache));
        Debug.WriteLine($"...saved {Cache.Servers.Count} servers");
        foreach (var s in Cache.Servers) Debug.WriteLine($"...{s.Id} {s.Hostname}:{s.Port}");
    }

    internal static Server GetCurrentServer()
        => Cache.GetServer(ServerId);

    internal static string GetServerInfoHeader(ServerHeaderTimestamp timestampType = ServerHeaderTimestamp.None)
    {
        if (string.IsNullOrEmpty(ServerId)) return "·· no server selected ··";
        var server = Cache.GetServer(ServerId);
        var timestamp = timestampType switch
        {
            ServerHeaderTimestamp.None => 
                string.Empty,

            ServerHeaderTimestamp.Visualizers => 
                server.RequestedVisualizersTimestamp.Equals(DateTime.MinValue) 
                ? string.Empty 
                : $"  ··  {server.RequestedVisualizersTimestamp:ddd MMM dd, yyyy h:mm tt}",

            ServerHeaderTimestamp.Playlists => 
                server.RequestedPlaylistsTimestamp.Equals(DateTime.MinValue) 
                ? string.Empty
                : $"  ··  {server.RequestedPlaylistsTimestamp:ddd MMM dd, yyyy h:mm tt}",
        };

        return $"·· {server.HostAndPort}{timestamp} ··";
    }
}
