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
    public class LocationService
    {
        private readonly TurismDbContext _context;

        public LocationService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<List<TravelLocation>> GetAllLocationsAsync()
        {
            return await _context.TravelLocations
                .Include(l => l.Tours)
                .ToListAsync();
        }

        public async Task<TravelLocation?> GetLocationByIdAsync(int id)
        {
            return await _context.TravelLocations
                .Include(l => l.Tours)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<TravelLocation>> GetLocationsByCountryAsync(string country)
        {
            return await _context.TravelLocations
                .Where(l => l.Country == country)
                .ToListAsync();
        }

        public async Task<bool> AddLocationAsync(TravelLocation location)
        {
            _context.TravelLocations.Add(location);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLocationAsync(TravelLocation location)
        {
            _context.TravelLocations.Update(location);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _context.TravelLocations.FindAsync(id);
            if (location == null) return false;

            _context.TravelLocations.Remove(location);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}