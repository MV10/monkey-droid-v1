
using CommandLineSwitchPipe;

namespace monkeydroid;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);
        if(OperatingSystem.IsWindows()) window.Created += OnWindowCreated;
        return window;
	}

    // Avoid the sloppy, gigantic default presentation on Windows
	private async void OnWindowCreated(object sender, EventArgs e)
	{
        const int defaultWidth = 800;
        const int defaultHeight = 800;

        var window = (Window)sender;
        window.Width = defaultWidth;
        window.Height = defaultHeight;
        window.X = -defaultWidth;
        window.Y = -defaultHeight;

        await window.Dispatcher.DispatchAsync(() => { });

        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
        window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
    }

}
