using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TurismMobile.Data;
using TurismMobile.Services;

namespace TurismMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "turism.db3");

            builder.Services.AddDbContext<TurismDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<TourService>();
            builder.Services.AddTransient<LocationService>();
            builder.Services.AddTransient<ReservationService>();
            builder.Services.AddTransient<ReviewService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}