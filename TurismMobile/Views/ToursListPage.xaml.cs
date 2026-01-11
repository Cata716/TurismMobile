using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

public partial class ToursListPage : ContentPage
{
    private readonly TourService _tourService;
    private readonly AuthService _authService;
    private List<Tour> _allTours;

    public ToursListPage(TourService tourService, AuthService authService)
    {
        InitializeComponent();
        _tourService = tourService;
        _authService = authService;
        _allTours = new List<Tour>();

        AddTourButton.IsVisible = _authService.IsAdmin;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadToursAsync();
    }

    private async Task LoadToursAsync()
    {
        _allTours = await _tourService.GetAvailableToursAsync();
        DisplayTours(_allTours);
    }

    private void DisplayTours(List<Tour> tours)
    {
        ToursContainer.Clear();

        if (!tours.Any())
        {
            ToursContainer.Add(new Label
            {
                Text = "Nu există tururi disponibile momentan.",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0),
                FontSize = 16,
                TextColor = Colors.Gray
            });
            return;
        }

        foreach (var tour in tours)
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                HasShadow = true,
                Padding = 15,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await OnTourTapped(tour);
            frame.GestureRecognizers.Add(tapGesture);

            var layout = new VerticalStackLayout { Spacing = 8 };

            layout.Add(new Label
            {
                Text = tour.Title,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#333333")
            });

            layout.Add(new Label
            {
                Text = $"📍 {tour.Location?.Name}, {tour.Location?.Country}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });

            layout.Add(new Label
            {
                Text = $"📅 {tour.StartDate:dd MMM yyyy} - {tour.EndDate:dd MMM yyyy}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });

            layout.Add(new Label
            {
                Text = $"💰 {tour.Price:F2} RON",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#2196F3")
            });

            var avgRating = tour.Reviews?.Any() == true ? tour.Reviews.Average(r => r.Rating) : 0;
            layout.Add(new Label
            {
                Text = $"⭐ {avgRating:F1} ({tour.Reviews?.Count ?? 0} recenzii)",
                FontSize = 14,
                TextColor = Color.FromArgb("#FF9800")
            });

            frame.Content = layout;
            ToursContainer.Add(frame);
        }
    }

    private async Task OnTourTapped(Tour tour)
    {
        await Shell.Current.GoToAsync($"TourDetailsPage?tourId={tour.Id}");
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            DisplayTours(_allTours);
            return;
        }

        var filtered = _allTours.Where(t =>
            t.Title.ToLower().Contains(searchText) ||
            t.Location?.Name.ToLower().Contains(searchText) == true ||
            t.Location?.Country.ToLower().Contains(searchText) == true
        ).ToList();

        DisplayTours(filtered);
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        var searchText = SearchBar.Text?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            DisplayTours(_allTours);
            return;
        }

        var filtered = _allTours.Where(t =>
            t.Title.ToLower().Contains(searchText) ||
            t.Location?.Name.ToLower().Contains(searchText) == true ||
            t.Location?.Country.ToLower().Contains(searchText) == true
        ).ToList();

        DisplayTours(filtered);
    }

    private async void OnAddTourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddEditTourPage));
    }
}