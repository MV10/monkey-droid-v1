
using monkeydroid.Content;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace monkeydroid.ViewModels;

internal class Playlist
{
    public ObservableCollection<PlaylistFile> Playlists { get; set; } = new();
}
