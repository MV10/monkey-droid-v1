
using monkeydroid.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace monkeydroid.Models;

internal class Visualizer
{
    [JsonIgnore]
    public string ServerInfoHeader { get => MauiProgram.GetServerInfoHeader(ServerHeaderTimestamp.Visualizers); }

    public ObservableCollection<VisualizerFile> Visualizers { get; set; } = new();
}
