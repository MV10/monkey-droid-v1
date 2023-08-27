
using monkeydroid.Content;
using monkeydroid.Utilities;
using System.Diagnostics;

namespace monkeydroid.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class ServerPage : ContentPage
{
    public string ItemId
    {
        set 
        {
            Debug.WriteLine($"ServerPage.ItemId {value}");
            LoadServer(value); 
        }
    }

	public ServerPage()
    {
        Debug.WriteLine("ServerPage.ctor");
        InitializeComponent();
        LoadServer();
    }

    private void LoadServer(string id = "")
    {
        var model = MauiProgram.Cache.Servers
            .FirstOrDefault(s => s.Id.Equals(id)) 
            ?? new Server();
        BindingContext = model;
        Debug.WriteLine($"ServerPage.LoadServer\trequested: {id}\treturned: {model.Id}");
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("ServerPage.SaveButton");

        if (BindingContext is Server server)
        {
            // Although XAML has validation support, it's a giant hassle to implement
            if (!await Validation.HostnameIsValid(server.Hostname))
            {
                txtHostname.Focus();
                return;
            }

            if (!await Validation.PortNumberIsValid(server.Port))
            {
                txtPort.Focus();
                return;
            }

            var index = server.GetIndexById();
            if (index == -1) MauiProgram.Cache.Servers.Add(server);
            MauiProgram.SaveCache();
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            Debug.WriteLine("ServerPage.SaveButton: BindingContext was not Models.Server type");
        }
    }

    private void txtHostname_Loaded(object sender, EventArgs e)
    {
        txtHostname.Focus();
    }

    // https://stackoverflow.com/a/76082113/152997
    private void Editor_Focused(object sender, FocusEventArgs e)
    {
        Dispatcher.Dispatch(() => 
        {
            var editor = sender as Editor;
            editor.CursorPosition = 0;
            editor.SelectionLength = editor.Text == null ? 0 : editor.Text.Length;
        });
    }
}
