using TurismMobile.Data;
using Microsoft.Maui.Controls;
using Plugin.LocalNotification;


namespace TurismMobile
{
    public partial class App : Application
    {
        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            Task.Run(async () => await databaseService.InitializeAsync()).Wait();

            MainPage = new AppShell();
            _ = RequestNotificationPermissionsAsync();
        }

        private async Task RequestNotificationPermissionsAsync()
        {
            try
            {
                // Verificăm dacă notificările sunt deja permise
                bool? areEnabled = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if (areEnabled == false || areEnabled == null)  // null = eroare / necunoscut
                {
                    // Cerem permisiunea → apare dialogul Android
                    bool granted = await LocalNotificationCenter.Current.RequestNotificationPermission();

                    if (!granted)
                    {
                        // Dacă user-ul refuză, poți arăta un mesaj frumos
                        await MainPage?.DisplayAlert(
                            "Notificări blocate",
                            "Pentru a primi reminder-e despre tururi și confirmări, trebuie să permiți notificările în setări.",
                            "OK");
                    }
                }


               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la cererea permisiunilor pentru notificări: {ex.Message}");
            }
        }
    }
}