
using CommandLineSwitchPipe;
using monkeydroid.ViewModels;
using monkeydroid.Utilities;
using monkeydroid.Content;

namespace monkeydroid.Views;

public partial class VisualizerPage : ContentPage
{
    private readonly Layout activityIndicator;

    public VisualizerPage()
	{
		InitializeComponent();
        activityIndicator = gridContent.AddActivityIndicator(0, 4);
    }

    protected override async void OnAppearing()
    {
        if (string.IsNullOrWhiteSpace(MauiProgram.ServerId))
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
        BindingContext = new Visualizer()
        {
            Visualizers = server.Visualizers
        };
    }

    private async void Refresh_Clicked(object sender, EventArgs e)
	{
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);
        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--md.list", "viz" }, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;

            if (success)
            {
                if (CommandLineSwitchServer.QueryResponse.StartsWith("ERR:"))
                {
                    await DisplayAlert("Refresh", $"Request failed.\n{CommandLineSwitchServer.QueryResponse}", "Ok");
                }
                else
                {
                    server.RequestedVisualizersTimestamp = DateTime.Now;
                    var results = CommandLineSwitchServer.QueryResponse.Split(CommandLineSwitchServer.Options.Advanced.SeparatorControlCode);
                    if (results.Length == 0)
                    {
                        await DisplayAlert("Refresh", "Request succeeded, but no visualizer filenames were returned.", "Ok");
                        server.Visualizers = new();
                    }
                    else
                    {
                        server.Visualizers = new(results.Select(v => new VisualizerFile() { Name = v }).ToList());
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

    private async void listVisualizers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        var filename = ((VisualizerFile)e.CurrentSelection[0]).Name;
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--load", filename }, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;
            if (!success) await DisplayAlert("Load Visualizer", "Failed to send command to the server. Is the server listening?", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Load Visualizer", $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
        }
        finally
        {
            activityIndicator.IsVisible = false;
            listVisualizers.SelectedItem = null;
        }
    }

    private async void Reload_Clicked(object sender, EventArgs e)
    {
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--reload" }, server.Hostname, server.PortNumber);
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
