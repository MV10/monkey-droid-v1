using CommandLineSwitchPipe;
using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class ControlsPage : ContentPage
{
    private readonly Layout activityIndicator;

    public ControlsPage()
	{
		InitializeComponent();
        activityIndicator = gridContent.AddActivityIndicator(0,6);
    }

    protected override async void OnAppearing()
	{
		if(string.IsNullOrWhiteSpace(MauiProgram.ServerId))
		{
			await DisplayAlert("Server Required", "Please add or select a server to use.", "Ok");
            await Shell.Current.GoToAsync("//serverlist");
        }
	}

    private async void Clicked_FPS(object sender, EventArgs e)
    {
        await SendCommand("--fps");
    }

    private async void Clicked_Info(object sender, EventArgs e)
    {
        await SendCommand("--info");
    }

    private async void Clicked_Next(object sender, EventArgs e)
    {
        await SendCommand("--next");
    }

    private async void Clicked_Reload(object sender, EventArgs e)
    {
        await SendCommand("--reload");
    }

    private async void Clicked_Idle(object sender, EventArgs e)
    {
        await SendCommand("--idle");
    }

    private async void Clicked_PID(object sender, EventArgs e)
    {
        await SendCommand("--pid");
    }

    private async void Clicked_Pause(object sender, EventArgs e)
    {
        await SendCommand("--pause");
    }

    private async void Clicked_Resume(object sender, EventArgs e)
    {
        await SendCommand("--run");
    }

    private async void Clicked_Log(object sender, EventArgs e)
    {
        await SendCommand("--log");
    }

    private async void Clicked_Quit(object sender, EventArgs e)
    {
        if(await DisplayAlert("Quit", $"Terminate monkey-hi-hat?", "Yes", "No"))
        {
            await SendCommand("--quit");
            MauiProgram.ServerId = string.Empty;
            await Shell.Current.GoToAsync("//serverlist");
        }
    }

    private async Task SendCommand(string command)
    {
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(new string[] { command }, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;
            if (success)
            {
                await DisplayAlert(command, CommandLineSwitchServer.QueryResponse, "Ok");
            }
            else
            {
                await DisplayAlert(command, "Failed to send command.", "Ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(command, $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}", "Ok");
        }
        finally
        {
            activityIndicator.IsVisible = false;
        }
    }
}