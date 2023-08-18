
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
            ?? new Models.Server();
        BindingContext = model;
        Debug.WriteLine($"ServerPage.LoadServer\trequested: {id}\treturned: {model.Id}");
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("ServerPage.SaveButton");
        if (BindingContext is Models.Server server)
        {
            // Although XAML has validation support, it's a giant hassle to implement
            if (!await Validation.HostnameIsValid(server.Hostname)) return;
            if (!await Validation.PortNumberIsValid(server.Port)) return;

            var index = server.GetIndexById();
            if (index > -1) MauiProgram.Cache.Servers.RemoveAt(index);
            MauiProgram.Cache.Servers.Add(server);
            MauiProgram.SaveCache();
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            Debug.WriteLine("ServerPage.SaveButton: BindingContext was not Models.Server type");
        }
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("ServerPage.DeleteButton");
        if (BindingContext is Models.Server server) 
        {
            server.DeleteFromCache();
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            Debug.WriteLine("ServerPage.SaveButton: BindingContext was not Models.Server type");
        }
    }
}
