
namespace monkeydroid.Models;

internal class About
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MonkeyDroidRepo => "https://github.com/MV10/monkey-droid";
    public string MonkeyHiHatRepo => "https://github.com/MV10/monkey-hi-hat";
    public string Message => "Remote control for the Monkey-Hi-Hat music visualizer.";
}
