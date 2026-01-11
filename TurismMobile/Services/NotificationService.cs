using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using TurismMobile.Data;
using Microsoft.EntityFrameworkCore;

namespace TurismMobile.Services
{
    public class NotificationService
    {
        private readonly TurismDbContext _context;

        public NotificationService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task SendReservationConfirmationAsync(int tourId, string tourTitle, int numberOfPeople, decimal totalPrice)
        {
            try
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1000 + new Random().Next(1000),
                    Title = "🎉 Rezervare Confirmată!",
                    Description = $"Tur: {tourTitle}\nPersone: {numberOfPeople}\nTotal: {totalPrice:F2} RON",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(2)
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare notificare: {ex.Message}");
            }
        }

        public async Task ScheduleTourReminderAsync(int reservationId, string tourTitle, DateTime tourStartDate)
        {
            try
            {
                var reminderTime = tourStartDate.AddDays(-1);

                if (reminderTime <= DateTime.Now)
                {
                    
                    reminderTime = DateTime.Now.AddSeconds(10);
                }

                var request = new NotificationRequest
                {
                    NotificationId = 2000 + reservationId,
                    Title = " Reminder: Tur Mâine!",
                    Description = $"Turul '{tourTitle}' începe mâine la ora {tourStartDate:HH:mm}. Pregătește-te pentru aventură!",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = reminderTime
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare programare reminder: {ex.Message}");
            }
        }

        // Notificare de test (pentru verificare)
        public async Task SendTestNotificationAsync()
        {
            try
            {
                var request = new NotificationRequest
                {
                    NotificationId = 9999,
                    Title = "🔔 Test Notificare",
                    Description = "Dacă vezi asta, notificările funcționează perfect!",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(3)
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare test notificare: {ex.Message}");
            }
        }
    }
}