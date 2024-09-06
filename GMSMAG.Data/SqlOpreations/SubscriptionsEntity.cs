using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public class SubscriptionsEntity : IDataHelper<Subscription>
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionsEntity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new subscription
        public async Task AddAsync(Subscription subscription)
        {
            try
            {
                await _dbContext.Subscriptions.AddAsync(subscription);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscription", ex);
            }
        }

        // Bulk Add for subscriptions
        public async Task AddBulkAsync(List<Subscription> subscriptions)
        {
            try
            {
                await _dbContext.Subscriptions.AddRangeAsync(subscriptions);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscriptions in bulk", ex);
            }
        }

        // Delete a subscription by ID
        public async Task DeleteAsync(int id)
        {
            try
            {
                var subscription = await _dbContext.Subscriptions.FindAsync(id);
                if (subscription != null)
                {
                    _dbContext.Subscriptions.Remove(subscription);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Subscription not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscription", ex);
            }
        }

        // Bulk delete for subscriptions
        public async Task DeleteBulkAsync(List<int> ids)
        {
            try
            {
                var subscriptions = await _dbContext.Subscriptions
                    .Where(s => ids.Contains(s.Id))
                    .ToListAsync();

                if (subscriptions.Any())
                {
                    _dbContext.Subscriptions.RemoveRange(subscriptions);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("No subscriptions found for the provided IDs");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscriptions in bulk", ex);
            }
        }

        // Edit an existing subscription
        public async Task EditAsync(Subscription subscription)
        {
            try
            {
                _dbContext.Subscriptions.Update(subscription);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing subscription", ex);
            }
        }

        // Find a subscription by ID
        public async Task<Subscription> FindAsync(int id)
        {
            try
            {
                var subscription = await _dbContext.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subscription == null)
                    throw new KeyNotFoundException("Subscription not found");

                return subscription;
            }
            catch (Exception ex)
            {
                throw new Exception("Error finding subscription", ex);
            }
        }

        // Get all subscriptions with paging
        public async Task<List<Subscription>> GetAllAsync(int page = 1, int limit = 50)
        {
            try
            {
                return await _dbContext.Subscriptions
                    .AsNoTracking()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving subscriptions", ex);
            }
        }

        // Search subscriptions with dynamic column selection and paging
        public async Task<List<Subscription>> SearchAsync(string term, string colName = "Id", int page = 1, int limit = 50)
        {
            try
            {
                var query = _dbContext.Subscriptions.AsQueryable();

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
                throw new Exception("Error searching subscriptions", ex);
            }
        }
    }
}
