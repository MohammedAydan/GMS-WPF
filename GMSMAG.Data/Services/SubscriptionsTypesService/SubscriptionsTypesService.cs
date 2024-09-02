using GMSMAG.Models;
using GMSMAG.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscriptionsTypesService
{
    public class SubscriptionsTypesService : ISubscriptionsTypesService
    {
        private readonly AppDbContext _context;

        public SubscriptionsTypesService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<SubscriptionsTypes> AddSubscriptionsTypeAsync(SubscriptionsTypes subscriptionsType)
        {
            if (subscriptionsType == null)
                throw new ArgumentNullException(nameof(subscriptionsType));

            await _context.SubscriptionsTypes.AddAsync(subscriptionsType);
            await _context.SaveChangesAsync();
            return subscriptionsType;
        }

        public async Task DeleteSubscriptionsTypeAsync(int id)
        {
            var subscriptionsType = await _context.SubscriptionsTypes.FindAsync(id);
            if (subscriptionsType != null)
            {
                _context.SubscriptionsTypes.Remove(subscriptionsType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SubscriptionsTypes?> GetSubscriptionsTypeAsync(int id)
        {
            return await _context.SubscriptionsTypes.FindAsync(id);
        }

        public async Task<IEnumerable<SubscriptionsTypes>> GetSubscriptionsTypesAsync(int page = 1, int limit = 30)
        {
            return await _context.SubscriptionsTypes
                .OrderBy(st => st.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<SubscriptionsTypes> UpdateSubscriptionsTypeAsync(SubscriptionsTypes subscriptionsType)
        {
            if (subscriptionsType == null)
                throw new ArgumentNullException(nameof(subscriptionsType));

            _context.Entry(subscriptionsType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subscriptionsType;
        }
    }
}