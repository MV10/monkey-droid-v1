using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private async void Clicked_MonkeyDroid(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.About about)
            await Launcher.Default.OpenAsync(about.MonkeyDroidRepo);
    }

    private async void Clicked_MonkeyHiHat(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.About about)
            await Launcher.Default.OpenAsync(about.MonkeyHiHatRepo);
    }

    private async void Clicked_DeleteCache(object sender, EventArgs e)
    {
        if (await DisplayAlert("Delete?", "Do you wish to delete all cached server information?", "Delete", "Cancel"))
        {
            await MauiProgram.ClearCache();
            if (File.Exists(ServerCache.Pathname())) File.Delete(ServerCache.Pathname());
            await Shell.Current.GoToAsync("//serverlist");
        }
    }
}