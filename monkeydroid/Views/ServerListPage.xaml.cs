
using CommandLineSwitchPipe;
using monkeydroid.Utilities;
using System.Diagnostics;

namespace monkeydroid.Views;

public partial class ServerListPage : ContentPage
{
    private readonly Layout activityIndicator;

	public ServerListPage()
	{
        Debug.WriteLine("ServerListPage.ctor");
		InitializeComponent();
        activityIndicator = gridContent.AddActivityIndicator(0, 3);
    }

    protected override void OnAppearing()
    {
        Debug.WriteLine("ServerListPage.OnAppearing");
        MauiProgram.LoadCache();
        BindingContext = new Models.ServerList();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("ServerListPage.Add");
        await Shell.Current.GoToAsync("server");
    }

    private async void listServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        try
        {
            var server = (Models.Server)e.CurrentSelection[0];
            Debug.WriteLine($"ServerListPage.Selection\tid {server.Id}");

            var action = await DisplayActionSheet($"Server {server.Hostname}:{server.Port}", "Cancel", "Delete", "Use", "Test", "Edit");

            // Bug: clicking outside DisplayActionSheet closes it and returns null
            // https://github.com/dotnet/maui/issues/15671
            if (string.IsNullOrEmpty(action)) return;

            if (action.Equals("Delete"))
            {
                if (await DisplayAlert("Delete?", $"Confirm you wish to delete {server.Hostname}", "Ok", "Cancel"))
                {
                    server.DeleteFromCache();
                    BindingContext = new Models.ServerList();
                }
                return;
            }

            if (action.Equals("Edit"))
            {
                await Shell.Current.GoToAsync($"server?{nameof(ServerPage.ItemId)}={server.Id}");
                return;
            }

            if (!await Validation.HostnameIsValid(server.Hostname)) return;
            if (!await Validation.PortNumberIsValid(server.Port)) return;

            if (action.Equals("Use"))
            {
                MauiProgram.ServerId = server.Id;
                await Shell.Current.GoToAsync("//playlist");
                return;
            }

            if (action.Equals("Test"))
            {
                try
                {
                    activityIndicator.IsVisible = true;
                    var success = await CommandLineSwitchServer.TryConnect(server.Hostname, server.PortNumber);
                    activityIndicator.IsVisible = false;

                    await DisplayAlert((success ? "Success" : "Failed"), $"{server.HostAndPort} is {(success ? "" : "not ")}listening for commands.", "Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", $"Error connecting to {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
                }
                finally
                {
                    activityIndicator.IsVisible = false;
                }
            }
        }
        finally
        {
            listServers.SelectedItem = null;
        }
    }
}
