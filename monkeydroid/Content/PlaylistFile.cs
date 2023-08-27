namespace monkeydroid.Content;

// Since the application can't change the Name property
// once it is set (and before these are added to the
// ObservableCollection), there is no need to implement
// change notification.

internal class PlaylistFile
{
    public string Name { get; set; } = string.Empty;
}
