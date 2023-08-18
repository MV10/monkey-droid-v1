namespace monkeydroid;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("server", typeof(Views.ServerPage));
    }
}
