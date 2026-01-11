using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

public partial class MyReservationsPage : ContentPage
{
    private readonly ReservationService _reservationService;
    private readonly AuthService _authService;

    public MyReservationsPage(ReservationService reservationService, AuthService authService)
    {
        InitializeComponent();
        _reservationService = reservationService;
        _authService = authService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadReservationsAsync();
    }

    private async Task LoadReservationsAsync()
    {
        List<Reservation> reservations;

        if (_authService.IsAdmin)
        {
            reservations = await _reservationService.GetAllReservationsAsync();
        }
        else
        {
            reservations = await _reservationService.GetUserReservationsAsync(_authService.CurrentUser.Id);
        }

        DisplayReservations(reservations);
    }

    private void DisplayReservations(List<Reservation> reservations)
    {
        ReservationsContainer.Clear();

        if (!reservations.Any())
        {
            ReservationsContainer.Add(new Label
            {
                Text = "Nu aveți rezervări active.",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0),
                FontSize = 16,
                TextColor = Colors.Gray
            });
            return;
        }

        foreach (var reservation in reservations)
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                HasShadow = true,
                Padding = 20
            };

            var layout = new VerticalStackLayout { Spacing = 8 };

            layout.Add(new Label
            {
                Text = reservation.Tour?.Title ?? "Tur necunoscut",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#333333")
            });

            if (_authService.IsAdmin)
            {
                layout.Add(new Label
                {
                    Text = $"👤 Client: {reservation.User?.FullName}",
                    FontSize = 14,
                    TextColor = Color.FromArgb("#666666")
                });
            }

            layout.Add(new Label
            {
                Text = $"📍 {reservation.Tour?.Location?.Name}, {reservation.Tour?.Location?.Country}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });

            layout.Add(new Label
            {
                Text = $"📅 {reservation.Tour?.StartDate:dd MMM yyyy} - {reservation.Tour?.EndDate:dd MMM yyyy}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });

            layout.Add(new Label
            {
                Text = $"👥 Număr persoane: {reservation.NumberOfPeople}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });

            layout.Add(new Label
            {
                Text = $"💰 Total: {reservation.TotalPrice:F2} RON",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#2196F3")
            });

            var statusColor = reservation.Status switch
            {
                "Confirmată" => "#4CAF50",
                "În așteptare" => "#FF9800",
                "Anulată" => "#F44336",
                _ => "#9E9E9E"
            };

            layout.Add(new Label
            {
                Text = $"Status: {reservation.Status}",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb(statusColor)
            });

            layout.Add(new Label
            {
                Text = $"Rezervat la: {reservation.BookingDate:dd MMM yyyy HH:mm}",
                FontSize = 12,
                TextColor = Colors.Gray
            });

            // Buton de anulare
            if (reservation.Status != "Anulată")
            {
                var cancelButton = new Button
                {
                    Text = "Anulează Rezervare",
                    BackgroundColor = Color.FromArgb("#F44336"),
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                cancelButton.Clicked += async (s, e) => await OnCancelReservation(reservation.Id);
                layout.Add(cancelButton);
            }

            frame.Content = layout;
            ReservationsContainer.Add(frame);
        }
    }

    private async Task OnCancelReservation(int reservationId)
    {
        bool confirm = await DisplayAlert("Confirmare", "Sigur doriți să anulați această rezervare?", "Da", "Nu");

        if (confirm)
        {
            if (await _reservationService.CancelReservationAsync(reservationId))
            {
                await DisplayAlert("Succes", "Rezervare anulată!", "OK");
                await LoadReservationsAsync();
            }
        }
    }
}