using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscribersService
{
    public class SubscribtionService : ISubscribtionService
    {
        private readonly AppDbContext _context;

        public SubscribtionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> AddSubscription(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptions(int page = 1, int limit = 30)
        {
            var subscriptions = await _context.Subscriptions
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return subscriptions;
        }

        public async Task<Subscription?> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            return subscription;
        }

        public async Task<Subscription> UpdateSubscription(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }
    }
}
