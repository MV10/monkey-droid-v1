
using monkeydroid.Content;
using monkeydroid.Utilities;
using System.Collections.ObjectModel;

namespace monkeydroid.ViewModels;

internal class Playlist
{
    public string ServerInfoHeader { get => MauiProgram.GetServerPageHeader(ServerHeaderTimestamp.Playlists); }

    public ObservableCollection<PlaylistFile> Playlists { get; set; } = new();
}
