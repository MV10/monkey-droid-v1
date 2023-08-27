
using monkeydroid.Content;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace monkeydroid.ViewModels;

internal class ServerList
{
    public ObservableCollection<Server> Servers { get; private set; } = MauiProgram.Cache.Servers;

    public ServerList()
    {
        Debug.WriteLine("ServerList.ctor");
        MauiProgram.LoadCache();
    }
}
