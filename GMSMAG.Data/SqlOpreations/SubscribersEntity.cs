using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public class SubscribersEntity : IDataHelper<Subscriber>
    {
        private AppDbContext _dbContext;

        public SubscribersEntity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new subscriber to the database
        public async Task AddAsync(Subscriber subscriber)
        {
            try
            {
                await _dbContext.Subscribers.AddAsync(subscriber);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("An error occurred while adding the subscriber.", ex);
            }
        }

        // Delete a subscriber by Id
        public async Task DeleteAsync(int Id)
        {
            try
            {
                var subscriber = await _dbContext.Subscribers.FindAsync(Id);
                if (subscriber != null)
                {
                    _dbContext.Subscribers.Remove(subscriber);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Subscriber not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("An error occurred while deleting the subscriber.", ex);
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
                // Handle the exception or log it
                throw new Exception("An error occurred while updating the subscriber.", ex);
            }
        }

        // Find a subscriber by Id
        public async Task<Subscriber> FindAsync(int Id)
        {
            try
            {
                return await _dbContext.Subscribers.FindAsync(Id)
                    ?? throw new KeyNotFoundException("Subscriber not found.");
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("An error occurred while finding the subscriber.", ex);
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
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("An error occurred while retrieving subscribers.", ex);
            }
        }

        // Search for subscribers based on a search term with dynamic column selection
        public async Task<List<Subscriber>> SearchAsync(string searchTerm, string colName = "Id", int page = 1, int limit = 50)
        {
            try
            {
                IQueryable<Subscriber> query = _dbContext.Subscribers.AsNoTracking();

                // Dynamically select the column to search by
                query = colName switch
                {
                    "Id" => query.Where(s => s.Equals(searchTerm)),
                    "FirstName" => query.Where(s => EF.Functions.Like(s.FirstName, $"%{searchTerm}%")),
                    "LastName" => query.Where(s => EF.Functions.Like(s.LastName, $"%{searchTerm}%")),
                    "PhoneNumber" => query.Where(s => s.Equals(searchTerm)),
                    "HomePhoneNumber" => query.Where(s => s.Equals(searchTerm)),
                    _ => query.Where(s => s.Id.ToString() == searchTerm),
                };

                return await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("An error occurred while searching for subscribers.", ex);
            }
        }
    }
}
