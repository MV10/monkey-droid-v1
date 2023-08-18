using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class ControlsPage : ContentPage
{
    private readonly Layout activityIndicatorLayout;

    public ControlsPage()
	{
		InitializeComponent();
        activityIndicatorLayout = contentLayout.AddActivityIndicator();
    }

    protected override async void OnAppearing()
	{
		if(string.IsNullOrWhiteSpace(MauiProgram.ServerId))
		{
			await DisplayAlert("Server Required", "Please add or select a server to use.", "Ok");
            await Shell.Current.GoToAsync("//serverlist");
        }
	}
}