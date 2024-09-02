using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Data.SqlOpreations
{
    public class SubscriptionsTypesEntity: IDataHelper<SubscriptionsTypes>
    {
        private AppDbContext _dbContext;

        public SubscriptionsTypesEntity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(SubscriptionsTypes subscriptionsTypes)
        {
            try
            {
                await _dbContext.SubscriptionsTypes.AddAsync(subscriptionsTypes);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the subscription type.", ex);
            }
        }

        public async Task DeleteAsync(int Id)
        {
            try
            {
                var subscriptionType = await _dbContext.SubscriptionsTypes.FindAsync(Id);
                if (subscriptionType != null)
                {
                    _dbContext.SubscriptionsTypes.Remove(subscriptionType);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Subscription type not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the subscription type.", ex);
            }
        }

        public async Task EditAsync(SubscriptionsTypes subscriptionsTypes)
        {
            try
            {
                _dbContext.SubscriptionsTypes.Update(subscriptionsTypes);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the subscription type.", ex);
            }
        }

        public async Task<SubscriptionsTypes> FindAsync(int Id)
        {
            try
            {
                return await _dbContext.SubscriptionsTypes.FindAsync(Id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the subscription type.", ex);
            }
        }

        public async Task<List<SubscriptionsTypes>> GetAllAsync(int Page = 1, int Limit = 50)
        {
            try
            {
                return await _dbContext.SubscriptionsTypes.Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all subscription types.", ex);
            }
        }

        public async Task<List<SubscriptionsTypes>> SearchAsync(string Item, string ColName = "Id", int Page = 1, int Limit = 50)
        {
            try
            {
                return await _dbContext.SubscriptionsTypes
                    .Where(x => EF.Property<string>(x, ColName).Contains(Item))
                    .Skip((Page - 1) * Limit)
                    .Take(Limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for subscription types.", ex);
            }
        }
    }
}
