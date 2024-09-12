using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public class SubscriptionsTypesEntity : IDataHelper<SubscriptionType>
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionsTypesEntity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new subscription type
        public async Task AddAsync(SubscriptionType subscriptionType)
        {
            try
            {
                await _dbContext.SubscriptionsTypes.AddAsync(subscriptionType);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscription type", ex);
            }
        }

        // Bulk Add for subscription types
        public async Task AddBulkAsync(List<SubscriptionType> subscriptionTypes)
        {
            try
            {
                await _dbContext.SubscriptionsTypes.AddRangeAsync(subscriptionTypes);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding subscription types in bulk", ex);
            }
        }

        // Delete a subscription type by ID
        public async Task DeleteAsync(int id)
        {
            try
            {
                var subscriptionType = await _dbContext.SubscriptionsTypes.FindAsync(id);
                if (subscriptionType != null)
                {
                    _dbContext.SubscriptionsTypes.Remove(subscriptionType);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Subscription type not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscription type", ex);
            }
        }

        // Bulk delete for subscription types
        public async Task DeleteBulkAsync(List<int> ids)
        {
            try
            {
                var subscriptionTypes = await _dbContext.SubscriptionsTypes
                    .Where(s => ids.Contains(s.Id))
                    .ToListAsync();

                if (subscriptionTypes.Any())
                {
                    _dbContext.SubscriptionsTypes.RemoveRange(subscriptionTypes);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("No subscription types found for the provided IDs");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting subscription types in bulk", ex);
            }
        }

        // Edit an existing subscription type
        public async Task EditAsync(SubscriptionType subscriptionType)
        {
            try
            {
                _dbContext.SubscriptionsTypes.Update(subscriptionType);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing subscription type", ex);
            }
        }

        // Find a subscription type by ID
        public async Task<SubscriptionType> FindAsync(int id)
        {
            try
            {
                var subscriptionType = await _dbContext.SubscriptionsTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subscriptionType == null)
                    throw new KeyNotFoundException("Subscription type not found");

                return subscriptionType;
            }
            catch (Exception ex)
            {
                throw new Exception("Error finding subscription type", ex);
            }
        }

        // Get all subscription types with paging
        public async Task<List<SubscriptionType>> GetAllAsync(int page = 1, int limit = 50)
        {
            try
            {
                return await _dbContext.SubscriptionsTypes
                    .AsNoTracking()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving subscription types", ex);
            }
        }

        // Search subscription types with dynamic column selection and paging
        public async Task<List<SubscriptionType>> SearchAsync(string term, string colName = "Id", int page = 1, int limit = 50)
        {
            try
            {
                
                var query = _dbContext.SubscriptionsTypes.AsQueryable();

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
                throw new Exception("Error searching subscription types", ex);
            }
        }
    }
}
