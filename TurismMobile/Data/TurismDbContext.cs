using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurismMobile.Models;

namespace TurismMobile.Data
{
    public class TurismDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TravelLocation> TravelLocations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public TurismDbContext(DbContextOptions<TurismDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "turism.db3");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Tour)
                .HasForeignKey(r => r.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Reviews)
                .WithOne(r => r.Tour)
                .HasForeignKey(r => r.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TravelLocation>()
                .HasMany(l => l.Tours)
                .WithOne(t => t.Location)
                .HasForeignKey(t => t.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}