using CommandLineSwitchPipe;
using Microsoft.Maui.Controls;
using monkeydroid.Utilities;

namespace monkeydroid.Views;

public partial class UtilitiesPage : ContentPage
{
    private readonly Layout activityIndicator;

    public UtilitiesPage()
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
        }
    }

    protected async void EntryCompleted(object sender, EventArgs e)
    {
        var text = entryCommand.Text.Trim().ToLowerInvariant();
        entryCommand.Text = string.Empty;
        if (text.Length == 0) return;

        if(text.Equals("cls"))
        {
            labelResponses.Text = string.Empty;
            return;
        }

        labelResponses.Text += $"\n> {text}\n";

        var args = text.Split(' ', StringSplitOptions.TrimEntries);
        var server = MauiProgram.Cache.GetServer(MauiProgram.ServerId);

        try
        {
            activityIndicator.IsVisible = true;
            var success = await CommandLineSwitchServer.TrySendArgs(args, server.Hostname, server.PortNumber);
            activityIndicator.IsVisible = false;
            if (success)
            {
                labelResponses.Text += CommandLineSwitchServer.QueryResponse;
            }
            else
            {
                labelResponses.Text += "Failed to send command.";
            }
        }
        catch (Exception ex)
        {
            labelResponses.Text += $"Error communicating with {server.HostAndPort}.\n{ex}: {ex.Message}";
        }
        finally
        {
            activityIndicator.IsVisible = false;
        }

        await Task.Delay(250); // give the label time to update
        await scrollView.ScrollToAsync(0, double.MaxValue, true);
    }
}