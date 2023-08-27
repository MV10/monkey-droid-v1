
using monkeydroid.Content;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace monkeydroid.Utilities;

internal class ServerCache
{
    public DateTime SavedTimestamp = DateTime.MinValue;

    public ObservableCollection<Server> Servers { get; set; } = new();

    public ServerCache()
    {
        Debug.WriteLine("CachedData.ctor");
        Clear();
    }

    public static string Pathname()
    {
        var path = FileSystem.AppDataDirectory;
        return Path.Combine(path, "cache.txt");
    }

    public void Clear()
    {
        Debug.WriteLine("CachedData.Clear");
        Servers.Clear();
    }

    public Server GetServer(string id)
        => Servers.FirstOrDefault(s => s.Id.Equals(id));
}
