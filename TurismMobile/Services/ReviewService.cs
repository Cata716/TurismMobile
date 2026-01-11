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
    public class ReviewService
    {
        private readonly TurismDbContext _context;

        public ReviewService(TurismDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Tour)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetTourReviewsAsync(int tourId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.TourId == tourId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetUserReviewsAsync(string userId)
        {
            return await _context.Reviews
                .Include(r => r.Tour)
                    .ThenInclude(t => t.Location)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            _context.Reviews.Add(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<double> GetAverageRatingForTourAsync(int tourId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.TourId == tourId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }
    }
}