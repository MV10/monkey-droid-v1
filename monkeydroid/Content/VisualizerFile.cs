
namespace monkeydroid.Content;

internal class VisualizerFile
{
    public static readonly string DefaultDescription = "(Description not loaded yet)";

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = DefaultDescription;

    public bool? UsesAudio { get; set; } = null;
}
