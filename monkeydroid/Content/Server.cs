using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace monkeydroid.Content;

internal class Server
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Hostname { get; set; }

    public string Port { get; set; }

    public DateTime RequestedPlaylistsTimestamp { get; set; } = DateTime.MinValue;

    public ObservableCollection<PlaylistFile> Playlists { get; set; } = new();

    public DateTime RequestedVisualizersTimestamp { get; set; } = DateTime.MinValue;

    public ObservableCollection<VisualizerFile> Visualizers { get; set; } = new();

    [JsonIgnore]
    public string HostAndPort { get => $"{Hostname}:{Port}"; }

    [JsonIgnore]
    public int PortNumber { get => int.TryParse(Port, out var port) ? port : -1; }

    public Server()
    {
        Debug.WriteLine($"Server.ctor");
    }

    public int GetIndexById()
    {
        var server = MauiProgram.Cache.GetServer(Id);
        var index = server is null ? -1 : MauiProgram.Cache.Servers.IndexOf(server);
        Debug.WriteLine($"Server.GetIndexById\tid: {Id}\tindex: {index}");
        return index;
    }

    public void DeleteFromCache()
    {
        var index = GetIndexById();
        if (index > -1)
        {
            MauiProgram.Cache.Servers.RemoveAt(index);
            MauiProgram.SaveCache();
        }
    }
}
