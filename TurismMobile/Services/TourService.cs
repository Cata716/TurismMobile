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
    public class TourService
    {
        private readonly TurismDbContext _context;

        public TourService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tour>> GetAllToursAsync()
        {
            return await _context.Tours
                .Include(t => t.Location)
                .Include(t => t.Reviews)
                .Include(t => t.Reservations)
                .ToListAsync();
        }

        public async Task<List<Tour>> GetAvailableToursAsync()
        {
            return await _context.Tours
                .Include(t => t.Location)
                .Include(t => t.Reviews)
                .Where(t => t.IsAvailable && t.StartDate > DateTime.Now)
                .ToListAsync();
        }

        public async Task<Tour?> GetTourByIdAsync(int id)
        {
            return await _context.Tours
                .Include(t => t.Location)
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.User)
                .Include(t => t.Reservations)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tour>> GetToursByLocationAsync(int locationId)
        {
            return await _context.Tours
                .Include(t => t.Location)
                .Where(t => t.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<bool> AddTourAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTourAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTourAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return false;

            _context.Tours.Remove(tour);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}