
using monkeydroid.Content;
using monkeydroid.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace monkeydroid.ViewModels;

internal class Visualizer
{
    [JsonIgnore]
    public string ServerInfoHeader { get => MauiProgram.GetServerPageHeader(ServerHeaderTimestamp.Visualizers); }

    public ObservableCollection<VisualizerFile> Visualizers { get; set; } = new();
}
