
using monkeydroid.Utilities;
using System.Collections.ObjectModel;

namespace monkeydroid.Models;

internal class Playlist
{
    public string ServerInfoHeader { get => MauiProgram.GetServerInfoHeader(ServerHeaderTimestamp.Playlists); }

    public ObservableCollection<PlaylistFile> Playlists { get; set; } = new();
}
