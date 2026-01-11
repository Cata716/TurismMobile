using TurismMobile.Models;
using TurismMobile.Services;

namespace TurismMobile.Views;

[QueryProperty(nameof(TourId), "tourId")]
public partial class TourDetailsPage : ContentPage
{
    private readonly TourService _tourService;
    private readonly ReviewService _reviewService;
    private readonly AuthService _authService;
    private Tour _currentTour;

    public string TourId { get; set; }

    public TourDetailsPage(TourService tourService, ReviewService reviewService, AuthService authService)
    {
        InitializeComponent();
        _tourService = tourService;
        _reviewService = reviewService;
        _authService = authService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTourDetailsAsync();
    }


    private async Task LoadTourDetailsAsync()
    {
        if (int.TryParse(TourId, out int id))
        {
            _currentTour = await _tourService.GetTourByIdAsync(id);

            if (_currentTour != null)
            {
                TitleLabel.Text = _currentTour.Title;
                LocationLabel.Text = $" {_currentTour.Location?.Name}, {_currentTour.Location?.Country}";
                DatesLabel.Text = $" {_currentTour.StartDate:dd MMMM yyyy} - {_currentTour.EndDate:dd MMMM yyyy}";
                DescriptionLabel.Text = _currentTour.Description;
                PriceLabel.Text = $" {_currentTour.Price:F2} RON / persoană";

                AdminButtons.IsVisible = _authService.IsAdmin;

                await LoadReviewsAsync();
            }
        }
    }

    private async Task LoadReviewsAsync()
    {
        ReviewsContainer.Clear();

        var reviews = await _reviewService.GetTourReviewsAsync(_currentTour.Id);

        if (!reviews.Any())
        {
            ReviewsContainer.Add(new Label
            {
                Text = "Nu există recenzii încă. Fii primul care adaugă o recenzie!",
                TextColor = Colors.Gray,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var review in reviews)
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                HasShadow = true,
                Padding = 15,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var layout = new VerticalStackLayout { Spacing = 5 };

            layout.Add(new Label
            {
                Text = review.User?.FullName ?? "Anonim",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#333333")
            });

            // Rating cu stele
            layout.Add(new Label
            {
                Text = new string('⭐', review.Rating),
                FontSize = 16,
                TextColor = Color.FromArgb("#FF9800")
            });

            if (!string.IsNullOrEmpty(review.Comment))
            {
                layout.Add(new Label
                {
                    Text = review.Comment,
                    FontSize = 14,
                    TextColor = Color.FromArgb("#666666"),
                    LineBreakMode = LineBreakMode.WordWrap,
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }

            layout.Add(new Label
            {
                Text = review.CreatedAt.ToString("dd MMM yyyy"),
                FontSize = 12,
                TextColor = Colors.Gray,
                Margin = new Thickness(0, 5, 0, 0)
            });

            frame.Content = layout;
            ReviewsContainer.Add(frame);
        }
    }

    private async void OnReserveClicked(object sender, EventArgs e)
    {
        if (_currentTour != null)
        {
            await Shell.Current.GoToAsync($"{nameof(AddReservationPage)}?tourId={_currentTour.Id}");
        }
    }

    private async void OnAddReviewClicked(object sender, EventArgs e)
    {
        if (_currentTour != null)
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditReviewPage)}?tourId={_currentTour.Id}");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (_currentTour != null)
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditTourPage)}?tourId={_currentTour.Id}");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmare",
            "Sigur doriți să ștergeți acest tur? Această acțiune nu poate fi anulată!",
            "Da, Șterge", "Anulează");

        if (confirm)
        {
            if (await _tourService.DeleteTourAsync(_currentTour.Id))
            {
                await DisplayAlert("Succes", "Tur șters cu succes!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Eroare", "Nu s-a putut șterge turul!", "OK");
            }
        }
    }
}