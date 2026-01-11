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
    public class UserService
    {
        private readonly TurismDbContext _context;

        public UserService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.Id = Guid.NewGuid().ToString();
            user.RegistrationDate = DateTime.UtcNow;
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
