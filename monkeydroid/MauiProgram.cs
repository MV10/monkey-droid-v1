using monkeydroid.Utilities;
using System.Diagnostics;
using System.Text.Json;
using monkeydroid.Content;

namespace monkeydroid;

public static class MauiProgram
{
	internal static ServerCache Cache = new();
    internal static string ServerId = string.Empty;

    private static CancellationTokenSource ctsBackgroundDetailReader = null;
    private static Task backgroundDetailReader = null;

	public static MauiApp CreateMauiApp()
	{
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

    // call this whenever the current server changes or the playlist is refreshed
    internal static async Task BeginReadVisualizerDetails()
    {
        if (backgroundDetailReader is not null) await AbortReadVisualizerDetails();

        var server = GetCurrentServer();
        if (server is null) return;

        ctsBackgroundDetailReader = new();
        var reader = new BackgroundDetailReader(server);
        var vizCopy = server.Visualizers.ToList();
        backgroundDetailReader = Task.Run(() => reader.RequestVisualizerDetailsAsync(vizCopy, ctsBackgroundDetailReader.Token));
    }

    // safe to call this without knowing if the read is actually happening
    internal static async Task AbortReadVisualizerDetails()
    {
        if (backgroundDetailReader is null || ctsBackgroundDetailReader is null) return;
        ctsBackgroundDetailReader.Cancel();
        await backgroundDetailReader;
        ctsBackgroundDetailReader = null;
        backgroundDetailReader = null;
    }

    internal static string GetServerPageHeader()
    {
        if (string.IsNullOrEmpty(ServerId)) return "·· no server selected ··";
        var server = Cache.GetServer(ServerId);
        var navigationRoute = Shell.Current.CurrentItem?.CurrentItem?.CurrentItem.Route ?? "null";
        var timestamp = navigationRoute switch
        {
            "viz" =>
                server.RequestedVisualizersTimestamp.Equals(DateTime.MinValue)
                ? string.Empty
                : $"  ··  {server.RequestedVisualizersTimestamp:ddd MMM dd, yyyy h:mm tt}",

            "playlist" =>
                server.RequestedPlaylistsTimestamp.Equals(DateTime.MinValue)
                ? string.Empty
                : $"  ··  {server.RequestedPlaylistsTimestamp:ddd MMM dd, yyyy h:mm tt}",

            _ => string.Empty,
        };

        return $"·· {server.HostAndPort}{timestamp} ··";
    }
}
