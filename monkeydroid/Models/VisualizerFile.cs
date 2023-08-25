
using System.Text.Json.Serialization;

namespace monkeydroid.Models;

internal class VisualizerFile
{
    public static readonly string DefaultDescription = "(Description not loaded yet)";

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = DefaultDescription;

    public bool UsesAudio { get; set; } = false;

    [JsonIgnore]
    public string AudioIcon { get => UsesAudio ? "\U000F075A " : "\U000F075B "; }
}
