using RestaurantSystem.Mobile.Services;

namespace RestaurantSystem.Mobile;

public partial class ReservationsPage : ContentPage
{
    private readonly ApiService _api = new ApiService();
    private readonly int _clientId;

    public ReservationsPage(int clientId)
    {
        InitializeComponent();
        _clientId = clientId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            var list = await _api.GetReservationsAsync(_clientId);
            ReservationsList.ItemsSource = list;

            EmptyLabel.IsVisible = (list.Count == 0);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", ex.Message, "OK");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadData();
    }
}
