
using CommandLineSwitchPipe;
using monkeydroid.Models;
using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class PlaylistPage : ContentPage
{
    private readonly Layout activityIndicator;

    public PlaylistPage()
	{
		InitializeComponent();
        activityIndicator = gridContent.AddActivityIndicator(0, 4);
    }

    protected override async void OnAppearing()
    {
        if (string.IsNullOrEmpty(MauiProgram.ServerId))
        {
            await DisplayAlert("Server Required", "Please add or select a server to use.", "Ok");
            await Shell.Current.GoToAsync("//serverlist");
            return;
        }

        SetBindingContext();
    }

    private void SetBindingContext()
    {
        var server = MauiProgram.GetCurrentServer();
        BindingContext = new Playlist()
        {
            Playlists = server.Playlists
        };
    }

    private async void Refresh_Clicked(object sender, EventArgs e)
    {
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);
        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--monkey.droid", "playlists" }, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;

            if (success)
            {
                if(CommandLineSwitchServer.QueryResponse.StartsWith("ERR:"))
                {
                    await DisplayAlert("Refresh", $"Request failed.\n{CommandLineSwitchServer.QueryResponse}", "Ok");
                }
                else
                {
                    server.RequestedPlaylistsTimestamp = DateTime.Now;
                    var results = CommandLineSwitchServer.QueryResponse.Split(CommandLineSwitchServer.Options.Advanced.SeparatorControlCode);
                    if(results.Length == 0)
                    {
                        await DisplayAlert("Refresh", "Request succeeded, but no playlist filenames were returned.", "Ok");
                        server.Playlists = new();
                    }
                    else
                    {
                        server.Playlists = new(results.Select(p => new PlaylistFile() { Name = p }).ToList());
                    }
                    MauiProgram.SaveCache();
                    SetBindingContext();
                }
            }
            else
            {
                await DisplayAlert("Refresh", "The server failed to receive or process the request.", "Ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Refresh", $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
        }
        finally
        {
            activityIndicator.IsVisible = false;
        }
    }

    private async void listPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        var filename = ((PlaylistFile)e.CurrentSelection[0]).Name;
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--playlist", filename }, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;
            if (!success) await DisplayAlert("Load Playlist", "Failed to send command to the server. Is the server listening?", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Load Playlist", $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
        }
        finally
        {
            activityIndicator.IsVisible = false;
            listPlaylists.SelectedItem = null;
        }
    }

    private async void NextViz_Clicked(object sender, EventArgs e)
    {
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--next" }, server.Hostname, server.PortNumber);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Next Viz", $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
        }
        finally
        {
            activityIndicator.IsVisible = false;
        }
    }
}
