using TurismMobile.Views;

namespace TurismMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ToursListPage), typeof(ToursListPage));
            Routing.RegisterRoute(nameof(TourDetailsPage), typeof(TourDetailsPage));
            Routing.RegisterRoute(nameof(AddEditTourPage), typeof(AddEditTourPage));
            Routing.RegisterRoute(nameof(LocationsListPage), typeof(LocationsListPage));
            Routing.RegisterRoute(nameof(AddEditLocationPage), typeof(AddEditLocationPage));
            Routing.RegisterRoute(nameof(MyReservationsPage), typeof(MyReservationsPage));
            Routing.RegisterRoute(nameof(AddReservationPage), typeof(AddReservationPage));
            Routing.RegisterRoute(nameof(AddEditReviewPage), typeof(AddEditReviewPage));
        
    }
    }
}