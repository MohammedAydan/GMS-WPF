using GMSMAG.Core;
using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSMAG.Data.SqlOpreations
{
    public class DashboardEntity : IDashboardEntity
    {
        private readonly AppDbContext _context;

        public DashboardEntity(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriberInfo>> GetExpiredSubscribersAsync(int page = 1, int limit = 50)
        {
            var currentDate = DateTime.UtcNow; // Use UTC to avoid timezone issues

            var expiredSubscribers = await _context.Subscriptions
                .AsNoTracking()
                .Join(
                    _context.Subscribers.AsNoTracking(),
                    subscription => subscription.SubscriberId,
                    subscriber => subscriber.Id,
                    (subscription, subscriber) => new
                    {
                        subscriber.Id,
                        subscriber.FirstName,
                        subscriber.LastName,
                        subscriber.PhoneNumber,
                        subscriber.HomePhoneNumber,
                        subscription.StartDate,
                        subscription.EndDate,
                        subscription.CreatedAt,
                    }
                )
                .Join(
                    _context.SubscriptionsTypes.AsNoTracking(),
                    subscription => subscription.Id,
                    subscriptionType => subscriptionType.Id,
                    (subscription, subscriptionType) => new SubscriberInfo
                    {
                        Id = subscription.Id,
                        FirstName = subscription.FirstName,
                        LastName = subscription.LastName,
                        PhoneNumber = subscription.PhoneNumber,
                        HomePhoneNumber = subscription.HomePhoneNumber,
                        StartDate = subscription.StartDate,
                        EndDate = subscription.EndDate,
                        CreatedAt = subscription.CreatedAt,
                        SubscriptionName = subscriptionType.SubscriptionName,
                        Price = subscriptionType.Price
                    }
                )
                .Where(s => s.EndDate < currentDate)
                .OrderBy(s => s.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return expiredSubscribers;
        }

        public async Task<int> GetSubscribersCountAsync()
        {
            return await _context.Subscribers.CountAsync();
        }

        public async Task<int> GetSubscriptionsCountAsync()
        {
            return await _context.Subscriptions.CountAsync();
        }

        public async Task<int> GetSubscriptionsTypesCountAsync()
        {
            return await _context.SubscriptionsTypes.CountAsync();
        }
    }
}
