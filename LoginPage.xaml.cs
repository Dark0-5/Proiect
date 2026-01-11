using RestaurantSystem.Mobile.Services;
//using static Android.Webkit.ConsoleMessage;

namespace RestaurantSystem.Mobile;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _api = new ApiService();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        MessageLabel.Text = "";

        var email = EmailEntry.Text?.Trim() ?? "";
        var password = PasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageLabel.Text = "Completeaza email si parola.";
            return;
        }

        try
        {
            var result = await _api.LoginAsync(email, password);

            if (!result.Success || result.ClientId == null)
            {
                MessageLabel.Text = result.Message;
                return;
            }

            // Navigheaza catre pagina de rezervari cu clientId
            await Navigation.PushAsync(new ReservationsPage(result.ClientId.Value));
        }
        catch (Exception ex)
        {
            MessageLabel.Text = "Eroare: " + ex.Message;
        }
    }
}
