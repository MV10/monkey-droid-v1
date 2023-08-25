
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace monkeydroid.Models;

internal class ServerList
{
    [JsonIgnore]
    public string ServerInfoHeader { get => MauiProgram.GetServerPageHeader(); }

    public ObservableCollection<Server> Servers { get; private set; } = MauiProgram.Cache.Servers;

    public ServerList()
    {
        Debug.WriteLine("ServerList.ctor");
        MauiProgram.LoadCache();
    }
}
