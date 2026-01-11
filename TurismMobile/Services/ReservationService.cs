using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TurismMobile.Data;
using TurismMobile.Models;

namespace TurismMobile.Services
{
    public class ReservationService
    {
        private readonly TurismDbContext _context;

        public ReservationService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Tour)
                    .ThenInclude(t => t.Location)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Tour)
                    .ThenInclude(t => t.Location)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.BookingDate)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Tour)
                    .ThenInclude(t => t.Location)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
{
    reservation.BookingDate = DateTime.UtcNow;
    _context.Reservations.Add(reservation);
    var result = await _context.SaveChangesAsync() > 0;
  
   // if (result)
   // {
   //     var notificationService = new NotificationService(_context);
   //     await notificationService.SendReservationConfirmationAsync(reservation.Id);
   // }
    
    return result;
}

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            reservation.Status = "Anulată";
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
