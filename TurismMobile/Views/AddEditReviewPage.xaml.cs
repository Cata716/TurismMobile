using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

[QueryProperty(nameof(TourId), "tourId")]
public partial class AddEditReviewPage : ContentPage
{
    private readonly ReviewService _reviewService;
    private readonly TourService _tourService;
    private readonly AuthService _authService;
    private int _rating = 0;
    private Tour _currentTour;

    public string TourId { get; set; }

    public AddEditReviewPage(ReviewService reviewService, TourService tourService, AuthService authService)
    {
        InitializeComponent();
        _reviewService = reviewService;
        _tourService = tourService;
        _authService = authService;
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
                TourNameLabel.Text = $"Tur: {_currentTour.Title}";
            }
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == Star1) _rating = 1;
        else if (button == Star2) _rating = 2;
        else if (button == Star3) _rating = 3;
        else if (button == Star4) _rating = 4;
        else if (button == Star5) _rating = 5;

        UpdateStars();
    }

    private void UpdateStars()
    {
        Star1.TextColor = _rating >= 1 ? Color.FromArgb("#FF9800") : Colors.Gray;
        Star2.TextColor = _rating >= 2 ? Color.FromArgb("#FF9800") : Colors.Gray;
        Star3.TextColor = _rating >= 3 ? Color.FromArgb("#FF9800") : Colors.Gray;
        Star4.TextColor = _rating >= 4 ? Color.FromArgb("#FF9800") : Colors.Gray;
        Star5.TextColor = _rating >= 5 ? Color.FromArgb("#FF9800") : Colors.Gray;

        RatingLabel.Text = _rating switch
        {
            1 => "⭐ Slab",
            2 => "⭐⭐ Acceptabil",
            3 => "⭐⭐⭐ Bun",
            4 => "⭐⭐⭐⭐ Foarte bun",
            5 => "⭐⭐⭐⭐⭐ Excelent",
            _ => "Selectează evaluarea"
        };
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (_rating == 0)
        {
            ErrorLabel.Text = "Vă rugăm să selectați o evaluare!";
            ErrorLabel.IsVisible = true;
            return;
        }

        var review = new Review
        {
            UserId = _authService.CurrentUser.Id,
            TourId = int.Parse(TourId),
            Rating = _rating,
            Comment = CommentEditor.Text?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        if (await _reviewService.AddReviewAsync(review))
        {
            await DisplayAlert("Succes", "Recenzie adăugată cu succes! Mulțumim!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ErrorLabel.Text = "Eroare la salvarea recenziei!";
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}