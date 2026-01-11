using TurismMobile.Services;
using TurismMobile.Views;
using TurismMobile.Models;

using TurismMobile.Services;

namespace TurismMobile;

public partial class MainPage : ContentPage
{
    private readonly AuthService _authService;

    public MainPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;

        LoadUserInfo();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        if (_authService.CurrentUser != null)
        {
            WelcomeLabel.Text = $"Bine ai venit, {_authService.CurrentUser.FirstName}!";
            UserRoleLabel.Text = $"Rol: {_authService.CurrentUser.Role}";

            // Afișează Admin Frame doar pentru admini
            AdminFrame.IsVisible = _authService.IsAdmin;

            // Afișează Profile Frame doar pentru useri normali
            ProfileFrame.IsVisible = !_authService.IsAdmin;
        }
    }

    private async void OnToursClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ToursListPage));
    }

    private async void OnLocationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LocationsListPage));
    }

    private async void OnReservationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyReservationsPage));
    }

    private async void OnAdminClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Administrare",
            "Aici poți gestiona:\n\n" +
            "• Tururi (Adaugă/Editează/Șterge)\n" +
            "• Locații (Adaugă/Editează/Șterge)\n" +
            "• Vezi toate rezervările\n" +
            "• Gestionează utilizatori",
            "OK");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        if (_authService.CurrentUser != null)
        {
            await DisplayAlert("Profil",
                $"Informații Cont\n\n" +
                $"Nume: {_authService.CurrentUser.FullName}\n" +
                $"Email: {_authService.CurrentUser.Email}\n" +
                $"Telefon: {(_authService.CurrentUser.Phone ?? "Necunoscut")}\n" +
                $"Rol: {_authService.CurrentUser.Role}\n" +
                $"Membru din: {_authService.CurrentUser.RegistrationDate:dd MMMM yyyy}",
                "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmare",
            "Sigur doriți să vă deconectați?",
            "Da", "Nu");

        if (confirm)
        {
            _authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }

}