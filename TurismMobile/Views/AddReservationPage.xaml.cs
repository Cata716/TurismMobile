using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

[QueryProperty(nameof(TourId), "tourId")]
public partial class AddReservationPage : ContentPage
{
    private readonly TourService _tourService;
    private readonly ReservationService _reservationService;
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private Tour _currentTour;
    private int _numberOfPeople = 1;

    public string TourId { get; set; }

    public AddReservationPage(TourService tourService, ReservationService reservationService, AuthService authService, NotificationService notificationService)
    {
        InitializeComponent();
        _tourService = tourService;
        _reservationService = reservationService;
        _authService = authService;
        _notificationService = notificationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTourAsync();
    }

    private async Task LoadTourAsync()
    {
        if (int.TryParse(TourId, out int id))
        {
            _currentTour = await _tourService.GetTourByIdAsync(id);

            if (_currentTour != null)
            {
                TourTitleLabel.Text = _currentTour.Title;
                TourLocationLabel.Text = $" {_currentTour.Location?.Name}, {_currentTour.Location?.Country}";
                TourDatesLabel.Text = $" {_currentTour.StartDate:dd MMM yyyy} - {_currentTour.EndDate:dd MMM yyyy}";
                TourPriceLabel.Text = $" {_currentTour.Price:F2} RON / persoană";

                UpdateTotalPrice();
            }
        }
    }

    private void OnDecreaseClicked(object sender, EventArgs e)
    {
        if (_numberOfPeople > 1)
        {
            _numberOfPeople--;
            UpdateDisplay();
        }
    }

    private void OnIncreaseClicked(object sender, EventArgs e)
    {
        if (_numberOfPeople < 50)
        {
            _numberOfPeople++;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        NumberOfPeopleLabel.Text = _numberOfPeople.ToString();
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        if (_currentTour != null)
        {
            var total = _currentTour.Price * _numberOfPeople;
            TotalPriceLabel.Text = $"{total:F2} RON";
        }
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        ConfirmButton.IsEnabled = false;
        ErrorLabel.IsVisible = false;

        try
        {
            if (_currentTour == null)
            {
                ErrorLabel.Text = "Eroare la încărcarea turului!";
                ErrorLabel.IsVisible = true;
                ConfirmButton.IsEnabled = true;
                return;
            }

            var reservation = new Reservation
            {
                UserId = _authService.CurrentUser.Id,
                TourId = _currentTour.Id,
                NumberOfPeople = _numberOfPeople,
                TotalPrice = _currentTour.Price * _numberOfPeople,
                Status = "Confirmată",
                BookingDate = DateTime.UtcNow
            };

            if (await _reservationService.AddReservationAsync(reservation))
            {
                await _notificationService.SendReservationConfirmationAsync(
                    _currentTour.Id,
                    _currentTour.Title,
                    _numberOfPeople,
                    reservation.TotalPrice
                );

                await _notificationService.ScheduleTourReminderAsync(
                    reservation.Id,
                    _currentTour.Title,
                    _currentTour.StartDate
                );


                await DisplayAlert("Succes",
                    $"Rezervare confirmată!\n\n" +
                    $"Tur: {_currentTour.Title}\n" +
                    $"Persoane: {_numberOfPeople}\n" +
                    $"Total: {reservation.TotalPrice:F2} RON",
                    "OK");

                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                ErrorLabel.Text = "Eroare la salvarea rezervării!";
                ErrorLabel.IsVisible = true;
                ConfirmButton.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o eroare: {ex.Message}", "OK");
            ConfirmButton.IsEnabled = true;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}