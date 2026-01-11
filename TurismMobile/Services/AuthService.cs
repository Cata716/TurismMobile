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
    public class AuthService
    {
        private readonly TurismDbContext _context;
        public User? CurrentUser { get; private set; }

        public AuthService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.Id = Guid.NewGuid().ToString();
            user.RegistrationDate = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsAuthenticated => CurrentUser != null;
        public bool IsAdmin => CurrentUser?.Role == "Admin";
    }
}
