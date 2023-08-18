using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class UtilitiesPage : ContentPage
{
    private readonly Layout activityIndicatorLayout;

    public UtilitiesPage()
	{
		InitializeComponent();
        activityIndicatorLayout = contentLayout.AddActivityIndicator();
    }

    protected override async void OnAppearing()
    {
        if (string.IsNullOrWhiteSpace(MauiProgram.ServerId))
        {
            await DisplayAlert("Server Required", "Please add or select a server to use.", "Ok");
            await Shell.Current.GoToAsync("//serverlist");
        }
    }

    private async void DeleteCache_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Delete?", "Do you wish to delete all cached server information?", "Delete", "Cancel"))
        {
            if (File.Exists(ServerCache.Pathname())) File.Delete(ServerCache.Pathname());
            MauiProgram.ServerId = string.Empty;
            await Shell.Current.GoToAsync("//serverlist");
        }
    }
}