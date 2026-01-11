using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

[QueryProperty(nameof(LocationId), "locationId")]
public partial class AddEditLocationPage : ContentPage
{
    private readonly LocationService _locationService;
    private TravelLocation _currentLocation;

    public string LocationId { get; set; }

    public AddEditLocationPage(LocationService locationService)
    {
        InitializeComponent();
        _locationService = locationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(LocationId) && int.TryParse(LocationId, out int id))
        {
            await LoadLocationAsync(id);
        }
    }

    private async Task LoadLocationAsync(int id)
    {
        _currentLocation = await _locationService.GetLocationByIdAsync(id);

        if (_currentLocation != null)
        {
            NameEntry.Text = _currentLocation.Name;
            CountryEntry.Text = _currentLocation.Country;
            DescriptionEditor.Text = _currentLocation.Description;
            DeleteButton.IsVisible = true;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(CountryEntry.Text))
        {
            ErrorLabel.Text = "Vă rugăm să completați câmpurile obligatorii!";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (_currentLocation == null)
        {
            var location = new TravelLocation
            {
                Name = NameEntry.Text.Trim(),
                Country = CountryEntry.Text.Trim(),
                Description = DescriptionEditor.Text?.Trim() ?? string.Empty
            };

            if (await _locationService.AddLocationAsync(location))
            {
                await DisplayAlert("Succes", "Locație adăugată cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        else
        {
            _currentLocation.Name = NameEntry.Text.Trim();
            _currentLocation.Country = CountryEntry.Text.Trim();
            _currentLocation.Description = DescriptionEditor.Text?.Trim() ?? string.Empty;

            if (await _locationService.UpdateLocationAsync(_currentLocation))
            {
                await DisplayAlert("Succes", "Locație actualizată cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmare",
            "Sigur doriți să ștergeți această locație? Toate tururile asociate vor fi șterse!",
            "Da", "Nu");

        if (confirm)
        {
            if (await _locationService.DeleteLocationAsync(_currentLocation.Id))
            {
                await DisplayAlert("Succes", "Locație ștearsă cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}