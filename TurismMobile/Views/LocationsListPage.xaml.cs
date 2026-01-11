using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

public partial class LocationsListPage : ContentPage
{
    private readonly LocationService _locationService;
    private readonly AuthService _authService;

    public LocationsListPage(LocationService locationService, AuthService authService)
    {
        InitializeComponent();
        _locationService = locationService;
        _authService = authService;

        AddLocationButton.IsVisible = _authService.IsAdmin;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadLocationsAsync();
    }

    private async Task LoadLocationsAsync()
    {
        var locations = await _locationService.GetAllLocationsAsync();
        DisplayLocations(locations);
    }

    private void DisplayLocations(List<TravelLocation> locations)
    {
        LocationsContainer.Clear();

        if (!locations.Any())
        {
            LocationsContainer.Add(new Label
            {
                Text = "Nu există locații înregistrate.",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0),
                FontSize = 16,
                TextColor = Colors.Gray
            });
            return;
        }

        foreach (var location in locations)
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                HasShadow = true,
                Padding = 20
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await OnLocationTapped(location);
            frame.GestureRecognizers.Add(tapGesture);

            var layout = new VerticalStackLayout { Spacing = 8 };

            layout.Add(new Label
            {
                Text = $"📍 {location.Name}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#333333")
            });

            layout.Add(new Label
            {
                Text = $"🌍 {location.Country}",
                FontSize = 16,
                TextColor = Color.FromArgb("#666666")
            });

            if (!string.IsNullOrEmpty(location.Description))
            {
                layout.Add(new Label
                {
                    Text = location.Description,
                    FontSize = 14,
                    TextColor = Color.FromArgb("#888888"),
                    MaxLines = 2,
                    LineBreakMode = LineBreakMode.TailTruncation
                });
            }

            layout.Add(new Label
            {
                Text = $"{location.Tours?.Count ?? 0} tururi disponibile",
                FontSize = 14,
                TextColor = Color.FromArgb("#2196F3"),
                FontAttributes = FontAttributes.Italic
            });

            frame.Content = layout;
            LocationsContainer.Add(frame);
        }
    }

    private async Task OnLocationTapped(TravelLocation location)
    {
        if (_authService.IsAdmin)
        {
            await Shell.Current.GoToAsync($"AddEditLocationPage?locationId={location.Id}");
        }
        else
        {
            await DisplayAlert("Locație", $"{location.Name}, {location.Country}\n\n{location.Description}", "OK");
        }
    }

    private async void OnAddLocationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddEditLocationPage");
    }
}