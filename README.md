# TurismMobile - Mobile Tourism Management Application

Cross-platform mobile application developed in .NET MAUI for managing tours, tourist locations, reservations, and reviews.

## Description

TurismMobile is a comprehensive mobile application that allows users to explore and manage tourism experiences. The application offers authentication features, tourist location viewing, reservation booking, review submission, and push notifications.

## Key Features

### Authentication and Authorization
- **Login** - Secure user authentication
- **Register** - New user registration with data validation
- **AuthService** - Centralized authentication management service

### Tourist Location Management
- **View locations** - Complete list of available tourist locations
- **Add locations** - Create new locations with complete details
- **Edit locations** - Modify existing location information
- **Delete locations** - Remove locations from the system

### Tour System
- **Tour list** - View available tours
- **Tour details** - Detailed information about each tour
- **Add tours** - Create new tours
- **Edit tours** - Modify existing tours

### Reservations
- **Create reservations** - Book tours
- **View reservations** - List of user's reservations
- **Edit reservations** - Modify reservation details
- **My Reservations** - Dedicated page for personal reservations

### Review System
- **Add reviews** - Users can leave feedback
- **Edit reviews** - Modify existing reviews
- **View reviews** - Browse reviews for tours/locations

### Notifications
- **NotificationService** - Push notification system
- **Custom notifications** - Alerts for reservations and important events

## Application Architecture

### Project Structure

```
TurismMobile/
├── Data/
│   ├── DatabaseService.cs          # Database operations service
│   └── TurismDbContext.cs          # Entity Framework context
├── Models/
│   ├── Reservation.cs              # Reservation model
│   ├── Review.cs                   # Review model
│   ├── Tour.cs                     # Tour model
│   ├── TravelLocation.cs           # Travel location model
│   └── User.cs                     # User model
├── Services/
│   ├── AuthService.cs              # Authentication service
│   ├── LocationService.cs          # Location management service
│   ├── NotificationService.cs      # Notification service
│   ├── ReservationService.cs       # Reservation service
│   ├── ReviewService.cs            # Review service
│   ├── TourService.cs              # Tour service
│   └── UserService.cs              # User service
├── Views/
│   ├── AddEditLocationPage.xaml    # Add/edit location page
│   ├── AddEditReviewPage.xaml      # Add/edit review page
│   ├── AddEditTourPage.xaml        # Add/edit tour page
│   ├── AddReservationPage.xaml     # Create reservation page
│   ├── LocationsListPage.xaml      # Locations list
│   ├── LoginPage.xaml              # Login page
│   ├── MyReservationsPage.xaml     # User's reservations
│   ├── RegisterPage.xaml           # Registration page
│   ├── TourDetailsPage.xaml        # Tour details
│   ├── ToursListPage.xaml          # Tours list
│   ├── MainPage.xaml               # Main page
│   └── AppShell.xaml               # Navigation shell
├── Platforms/
│   ├── Android/                    # Android-specific configurations
│   ├── iOS/                        # iOS-specific configurations
│   ├── MacCatalyst/                # macOS-specific configurations
│   ├── Tizen/                      # Tizen-specific configurations
│   └── Windows/                    # Windows-specific configurations
└── Resources/                       # App resources (images, fonts, etc.)
```

## Technologies Used

- **.NET MAUI** - Cross-platform framework for mobile applications
- **C#** - Primary programming language
- **Entity Framework Core** - ORM for database interaction
- **SQLite** - Local database
- **XAML** - Markup language for user interface

## Database

The application uses the following main entities:

### Tables

1. **Users** - User information
   - Id (PK) - string (GUID)
   - FirstName
   - LastName
   - Email
   - PasswordHash
   - BirthDate (nullable)
   - Phone
   - Address
   - Role (default: "User")
   - RegistrationDate

2. **TravelLocations** - Tourist locations
   - Id (PK)
   - Name
   - Country
   - Description

3. **Tours** - Available tours
   - Id (PK)
   - Title
   - Description
   - Price
   - StartDate
   - EndDate
   - IsAvailable
   - LocationId (FK)

4. **Reservations** - Bookings made
   - Id (PK)
   - UserId (FK)
   - TourId (FK)
   - BookingDate
   - NumberOfPeople (1-50)
   - TotalPrice
   - Status (default: "În așteptare")

5. **Reviews** - User reviews
   - Id (PK)
   - UserId (FK)
   - TourId (FK)
   - Rating (1-4 stars)
   - Comment (max 1000 chars)
   - CreatedAt
   - IsHelpful (boolean)

### Table Relationships

- **Users** 1:N **Reservations** (One user can have multiple reservations)
- **Users** 1:N **Reviews** (One user can write multiple reviews)
- **Tours** 1:N **Reservations** (One tour can have multiple reservations)
- **Tours** 1:N **Reviews** (One tour can have multiple reviews)
- **TravelLocations** 1:N **Tours** (One location can have multiple tours)

## Installation and Setup

### Prerequisites

- Visual Studio 2022 (version 17.8 or later)
- .NET 8.0 SDK or later
- .NET MAUI workload installed in Visual Studio
- (For Android) Android SDK and emulator/physical device
- (For iOS) Xcode and macOS device (for build)

### Installation Steps

1. **Clone repository**
   ```bash
   git clone https://github.com/Cata716/TurismMobile.git
   cd TurismMobile
   ```

2. **Open solution**
   - Open `TurismMobile.sln` in Visual Studio 2022

3. **Restore NuGet packages**
   - Visual Studio will automatically restore required packages
   - Or manually: Right-click on solution → Restore NuGet Packages

4. **Database configuration**
   - SQLite database will be created automatically on first run
   - Check database path in `DatabaseService.cs`

5. **Run application**
   - Select target platform (Android/iOS/Windows)
   - Press F5 or click the "Start" button

## Supported Platforms

- **Android** (API 21+)
- **iOS** (iOS 11+)
- **Windows** (Windows 10 1809+)
- **macOS** (macOS 10.15+)
- **Tizen** (Experimental)

## Implemented CRUD Operations

The application fully implements CRUD operations for all entities:

### Users
- Create - New user registration
- Read - Authentication and profile viewing
- Update - Profile information update
- Delete - User account deletion

### TravelLocations
- Create - Add new locations
- Read - View location list and details
- Update - Edit location information
- Delete - Remove locations

### Tours
- Create - Create new tours
- Read - View tour list and details
- Update - Modify existing tours
- Delete - Remove tours

### Reservations
- Create - Make new reservations
- Read - View own reservations
- Update - Modify reservation details
- Delete - Cancel reservations

### Reviews
- Create - Submit new reviews
- Read - View reviews
- Update - Edit own reviews
- Delete - Remove own reviews

## Data Validation

The application implements comprehensive validation for:

- **Email** - Valid email address format
- **Username** - Minimum length, allowed characters
- **Password** - Minimum complexity, length requirements
- **Reservations** - Valid dates, positive number of persons
- **Rating** - Values between 1-4
- **Dates** - Correct format, future dates for reservations

## Security

- **Authentication** - Secure login with password hashing
- **Authorization** - Permission verification for sensitive operations
- **Input validation** - User data sanitization
- **Session management** - User session handling



## License

This project is developed for educational purposes for the E-services Engineering course.

**Note**: This project is part of a larger information system that also includes an ASP.NET Core web application for tourism management.
