using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TurismMobile.Models;

namespace TurismMobile.Data
{
    public class DatabaseService
    {
        private readonly TurismDbContext _context;

        public DatabaseService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            if (!await _context.TravelLocations.AnyAsync())
            {
                _context.TravelLocations.AddRange(
                    new TravelLocation
                    {
                        Name = "Paris",
                        Country = "Franța",
                        Description = "Orașul Luminilor - celebru pentru Turnul Eiffel și Luvru"
                    },
                    new TravelLocation
                    {
                        Name = "Roma",
                        Country = "Italia",
                        Description = "Orașul Etern - plin de istorie și artă"
                    },
                    new TravelLocation
                    {
                        Name = "Barcelona",
                        Country = "Spania",
                        Description = "Oraș vibrant cu arhitectură unică de Gaudi"
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!await _context.Users.AnyAsync())
            {
                var adminPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
                var userPassword = BCrypt.Net.BCrypt.HashPassword("user123");

                _context.Users.AddRange(
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "System",
                        Email = "admin@turism.ro",
                        PasswordHash = adminPassword,
                        Role = "Admin",
                        Phone = "0740123456"
                    },
                    new User
                    {
                        FirstName = "Ion",
                        LastName = "Popescu",
                        Email = "ion@test.ro",
                        PasswordHash = userPassword,
                        Role = "User",
                        Phone = "0741234567"
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!await _context.Tours.AnyAsync())
            {
                var paris = await _context.TravelLocations.FirstAsync(l => l.Name == "Paris");
                var roma = await _context.TravelLocations.FirstAsync(l => l.Name == "Roma");

                _context.Tours.AddRange(
                    new Tour
                    {
                        Title = "Paris - City Break 3 zile",
                        Description = "Vizitați cele mai frumoase obiective din Paris",
                        Price = 599,
                        StartDate = DateTime.Now.AddDays(30),
                        EndDate = DateTime.Now.AddDays(33),
                        LocationId = paris.Id,
                        IsAvailable = true
                    },
                    new Tour
                    {
                        Title = "Roma - Descoperă istoria",
                        Description = "Tur ghidat prin Roma antică și Vatican",
                        Price = 799,
                        StartDate = DateTime.Now.AddDays(45),
                        EndDate = DateTime.Now.AddDays(49),
                        LocationId = roma.Id,
                        IsAvailable = true
                    }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}