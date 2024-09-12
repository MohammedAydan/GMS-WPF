using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public class SubscribersEntity : IDataHelper<Subscriber>
    {
        private readonly AppDbContext _dbContext;

        public SubscribersEntity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new subscriber
        public async Task AddAsync(Subscriber subscriber)
        {
            try
            {
                await _dbContext.Subscribers.AddAsync(subscriber);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscriber", ex);
            }
        }

        // Bulk Add for better performance with large data
        public async Task AddBulkAsync(List<Subscriber> subscribers)
        {
            try
            {
                await _dbContext.Subscribers.AddRangeAsync(subscribers);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscribers in bulk", ex);
            }
        }

        // Delete a subscriber by ID
        public async Task DeleteAsync(int id)
        {
            try
            {
                var subscriber = await _dbContext.Subscribers.FindAsync(id);
                if (subscriber != null)
                {
                    _dbContext.Subscribers.Remove(subscriber);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Subscriber not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscriber", ex);
            }
        }

        // Bulk Delete for large data
        public async Task DeleteBulkAsync(List<int> ids)
        {
            try
            {
                var subscribers = await _dbContext.Subscribers
                    .Where(s => ids.Contains(s.Id))
                    .ToListAsync();

                if (subscribers.Any())
                {
                    _dbContext.Subscribers.RemoveRange(subscribers);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("No subscribers found for the provided IDs");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscribers in bulk", ex);
            }
        }

        // Edit an existing subscriber
        public async Task EditAsync(Subscriber subscriber)
        {
            try
            {
                _dbContext.Subscribers.Update(subscriber);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing subscriber", ex);
            }
        }

        // Find subscriber by ID
        public async Task<Subscriber> FindAsync(int id)
        {
            try
            {
                var subscriber = await _dbContext.Subscribers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subscriber == null)
                    throw new KeyNotFoundException("Subscriber not found");

                return subscriber;
            }
            catch (Exception ex)
            {
                throw new Exception("Error finding subscriber", ex);
            }
        }

        // Get all subscribers with paging
        public async Task<List<Subscriber>> GetAllAsync(int page = 1, int limit = 50)
        {
            try
            {
                return await _dbContext.Subscribers
                    .AsNoTracking()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Include(i => i.Subscriptions.OrderByDescending(s => s.CreatedAt))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving subscribers", ex);
            }
        }

        // Search subscribers with dynamic column selection and paging
        public async Task<List<Subscriber>> SearchAsync(string term, string colName = "Id", int page = 1, int limit = 50)
        {
            try
            {
                var query = _dbContext.Subscribers.AsQueryable();

                if (colName == "Id")
                    query = query.Where(s => s.Id.ToString().Equals(term));
                else
                    query = query.Where(s => EF.Property<string>(s, colName).Contains(term));

                return await query
                    .AsNoTracking()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching subscribers", ex);
            }
        }
    }
}
