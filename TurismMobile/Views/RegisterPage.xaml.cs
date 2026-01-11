using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService _authService;

    public RegisterPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var firstName = FirstNameEntry.Text?.Trim();
        var lastName = LastNameEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var phone = PhoneEntry.Text?.Trim();
        var password = PasswordEntry.Text;
        var confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Vă rugăm să completați toate câmpurile obligatorii!";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (password.Length < 6)
        {
            ErrorLabel.Text = "Parola trebuie să aibă minim 6 caractere!";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (password != confirmPassword)
        {
            ErrorLabel.Text = "Parolele nu coincid!";
            ErrorLabel.IsVisible = true;
            return;
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone ?? string.Empty,
            Role = "User"
        };

        if (await _authService.RegisterAsync(user, password))
        {
            await DisplayAlert("Succes", "Cont creat cu succes! Vă puteți autentifica acum.", "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else
        {
            ErrorLabel.Text = "Acest email este deja înregistrat!";
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnBackToLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}