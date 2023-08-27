
using System.Diagnostics;
using System.Reflection;

namespace monkeydroid;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("server", typeof(Views.ServerPage));
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // This is a workaround for the fact that Page objects (but not their models)
    // are only ever instantiated once. It just saves the initial nothing-cached
    // state and re-applies it before every navigation event.
    // Bug report: https://github.com/dotnet/maui/issues/9300
    // Workaround: https://github.com/dotnet/maui/issues/9300#issuecomment-1416893792
    // CurrentPage bug report: https://github.com/dotnet/maui/issues/17019

    ShellContent contentWithoutCachedPages = null;

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if(CurrentItem?.CurrentItem?.CurrentItem is not null && contentWithoutCachedPages is not null)
        {
            var prop = typeof(ShellContent)
                .GetProperty("ContentCache",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            prop.SetValue(contentWithoutCachedPages, null);
        }

        contentWithoutCachedPages = CurrentItem?.CurrentItem?.CurrentItem;
    }

    // end of workaround
    ////////////////////////////////////////////////////////////////////////////////////////
}
