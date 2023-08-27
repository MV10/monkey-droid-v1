
using monkeydroid.Content;
using System.Collections.ObjectModel;

namespace monkeydroid.ViewModels;

internal class Visualizer
{
    public ObservableCollection<VisualizerFile> Visualizers { get; set; } = new();
}
