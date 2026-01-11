using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

[QueryProperty(nameof(TourId), "tourId")]
public partial class AddEditTourPage : ContentPage
{
    private readonly TourService _tourService;
    private readonly LocationService _locationService;
    private List<TravelLocation> _locations;
    private Tour _currentTour;

    public string TourId { get; set; }

    public AddEditTourPage(TourService tourService, LocationService locationService)
    {
        InitializeComponent();
        _tourService = tourService;
        _locationService = locationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadLocationsAsync();

        if (!string.IsNullOrEmpty(TourId) && int.TryParse(TourId, out int id))
        {
            await LoadTourAsync(id);
        }
    }

    private async Task LoadLocationsAsync()
    {
        _locations = await _locationService.GetAllLocationsAsync();
        LocationPicker.ItemsSource = _locations.Select(l => $"{l.Name}, {l.Country}").ToList();
    }

    private async Task LoadTourAsync(int id)
    {
        _currentTour = await _tourService.GetTourByIdAsync(id);

        if (_currentTour != null)
        {
            TitleEntry.Text = _currentTour.Title;
            DescriptionEditor.Text = _currentTour.Description;
            PriceEntry.Text = _currentTour.Price.ToString();
            StartDatePicker.Date = _currentTour.StartDate;
            EndDatePicker.Date = _currentTour.EndDate;
            IsAvailableSwitch.IsToggled = _currentTour.IsAvailable;

            var locationIndex = _locations.FindIndex(l => l.Id == _currentTour.LocationId);
            if (locationIndex >= 0)
            {
                LocationPicker.SelectedIndex = locationIndex;
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text) ||
            string.IsNullOrWhiteSpace(PriceEntry.Text) ||
            LocationPicker.SelectedIndex < 0)
        {
            ErrorLabel.Text = "Vă rugăm să completați toate câmpurile obligatorii!";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (!decimal.TryParse(PriceEntry.Text, out decimal price) || price <= 0)
        {
            ErrorLabel.Text = "Prețul trebuie să fie un număr pozitiv!";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (EndDatePicker.Date <= StartDatePicker.Date)
        {
            ErrorLabel.Text = "Data de sfârșit trebuie să fie după data de start!";
            ErrorLabel.IsVisible = true;
            return;
        }

        var selectedLocation = _locations[LocationPicker.SelectedIndex];

        if (_currentTour == null)
        {
            var tour = new Tour
            {
                Title = TitleEntry.Text.Trim(),
                Description = DescriptionEditor.Text?.Trim() ?? string.Empty,
                Price = price,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                LocationId = selectedLocation.Id,
                IsAvailable = IsAvailableSwitch.IsToggled
            };

            if (await _tourService.AddTourAsync(tour))
            {
                await DisplayAlert("Succes", "Tur adăugat cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        else
        {
            _currentTour.Title = TitleEntry.Text.Trim();
            _currentTour.Description = DescriptionEditor.Text?.Trim() ?? string.Empty;
            _currentTour.Price = price;
            _currentTour.StartDate = StartDatePicker.Date;
            _currentTour.EndDate = EndDatePicker.Date;
            _currentTour.LocationId = selectedLocation.Id;
            _currentTour.IsAvailable = IsAvailableSwitch.IsToggled;

            if (await _tourService.UpdateTourAsync(_currentTour))
            {
                await DisplayAlert("Succes", "Tur actualizat cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}