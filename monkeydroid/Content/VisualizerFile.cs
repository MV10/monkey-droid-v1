
using System.ComponentModel;

namespace monkeydroid.Content;

// Change-notification is implemented here (unlike the other Content
// classes in this program) because Description and UsesAudio can
// be changed by the BackgroundDetailReader class while VisualizerPage
// is active. Name can't be changed, so we don't bother with all the
// extra noise on that property.

// This shit should really be easier in this day and age:
// https://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist/

internal class VisualizerFile : INotifyPropertyChanged
{
    public static readonly string DefaultDescription = "(Description not loaded yet)";

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; } = string.Empty;

    private string description = DefaultDescription;
    public string Description 
    { 
        get => description; 
        set
        {
            if (value.Equals(description)) return;
            description = value;
            PropertyChanged?.Invoke(this, new(nameof(Description)));
        }
    }

    private bool? usesAudio = null;
    public bool? UsesAudio 
    { 
        get => usesAudio;
        set
        {
            if (value.Equals(usesAudio)) return;
            usesAudio = value;
            PropertyChanged?.Invoke(this, new(nameof(UsesAudio)));
        }
    }
}
