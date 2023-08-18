
namespace monkeydroid.Utilities;

internal static class Validation
{
    internal static async Task<bool> HostnameIsValid(string host)
    {
        if (!string.IsNullOrWhiteSpace(host)) return true;
        await Shell.Current.DisplayAlert("Error", "Please enter a valid server name.", "Ok");
        return false;
    }

    internal static async Task<bool> PortNumberIsValid(string port)
    {
        if (!int.TryParse(port, out var portnumber) || portnumber < 1 || portnumber > 65535)
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a valid port number (49152 to 65535 recommended).", "Ok");
            return false;
        }

        if (portnumber < 49152)
            return await Shell.Current.DisplayAlert("Save?", "Please confirm. Port numbers below 49152 may be reserved.", "Save", "Cancel");

        return true;
    }
}
