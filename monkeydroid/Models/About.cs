
namespace monkeydroid.Models;

internal class About
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MoreInfoUrl => "https://github.com/MV10/monkey-hi-hat";
    public string Message => "Remote control for the monkey-hi-hat music visualizer.";
}
