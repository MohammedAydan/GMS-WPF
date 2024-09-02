using GMSMAG.Core.Data;
using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GMSMAG.Services.SubscriptionsTypesService
{
    internal class SubscriberService : ISubscriberService
    {
        private readonly AppDbContext _context;

        public SubscriberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Subscriber> AddSubscriber(Subscriber subscriber)
        {
            await _context.Subscribers.AddAsync(subscriber);
            await _context.SaveChangesAsync();
            return subscriber;
        }

        public async Task DeleteSubscriber(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Subscriber not found.");
            }
        }

        public async Task<Subscriber?> GetSubscriber(int id)
        {
            return await _context.Subscribers
                .AsNoTracking()
                .Include(s => s.Subscriptions)  // Assuming it's a collection; adjust if it's a single navigation property
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscriber?> GetLastSubscriber(int id)
        {
            return await _context.Subscribers
                .AsNoTracking()
                .Include(s => s.Subscriptions)
                .Where(s => s.Id == id)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Subscriber>> GetSubscribers(int page = 1, int limit = 50)
        {
            return await _context.Subscribers
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Subscriber> UpdateSubscriber(Subscriber subscriber)
        {
            var existingSubscriber = await _context.Subscribers
                .Include(s => s.Subscriptions)  // Include related entities if necessary
                .FirstOrDefaultAsync(s => s.Id == subscriber.Id);

            if (existingSubscriber != null)
            {
                subscriber.AdminId = existingSubscriber.AdminId;

                _context.Entry(existingSubscriber).CurrentValues.SetValues(subscriber);

                // Handle related entities updates if necessary
                if (subscriber.Subscriptions != null)
                {
                    // Assuming Subscription is a collection; handle accordingly if it's different
                    foreach (var subscription in subscriber.Subscriptions)
                    {
                        var existingSubscription = existingSubscriber.Subscriptions
                            .FirstOrDefault(s => s.Id == subscription.Id);

                        if (existingSubscription != null)
                        {
                            _context.Entry(existingSubscription).CurrentValues.SetValues(subscription);
                        }
                        else
                        {
                            existingSubscriber.Subscriptions.Add(subscription);
                        }
                    }
                }

                _context.SaveChanges();
                return existingSubscriber;
            }
            else
            {
                throw new InvalidOperationException("Subscriber not found.");
            }
        }

        public async Task<List<Subscriber>> Search(string txt, string colName = "Id", int page = 1, int limit = 50)
        {
            IQueryable<Subscriber> query = _context.Subscribers;

            if (colName.Equals("SubscriberId-SubscriptionId"))
            {
                var ids = txt.Split('-');
                if (ids.Length != 2)
                    throw new ArgumentException("Invalid format for SubscriberId-SubscriptionId.");

                var subscriberId = ids[0];
                var subscriptionId = ids[1];

                // Find the subscriber with the given ID
                var subscriber = await query
                    .FirstOrDefaultAsync(s => s.Id.ToString() == subscriberId);

                if (subscriber == null)
                    throw new InvalidOperationException("Subscriber not found.");

                // Check if the subscriber has the subscription with the given ID
                var subscription = subscriber.Subscriptions
                    .FirstOrDefault(sub => sub.Id.ToString() == subscriptionId
                                           && sub.StartDate <= DateTime.Now
                                           && sub.EndDate >= DateTime.Now);

                if (subscription == null)
                    throw new InvalidOperationException("Subscription not found.");

                return new List<Subscriber> { subscriber };
            }
            else
            {
                // Apply filter based on the column name
                var filter = GetFilters(txt, colName);
                query = query.Where(filter);

                // Apply pagination
                query = query.Skip((page - 1) * limit).Take(limit);

                // Execute the query and return the results
                return await query.ToListAsync();
            }
        }

        private Expression<Func<Subscriber, bool>> GetFilters(string txt, string colName)
        {
            switch (colName)
            {
                case "Id":
                    return x => x.Id.ToString() == txt;
                case "FirstName":
                    return x => x.FirstName.Contains(txt);
                case "LastName":
                    return x => x.LastName.Contains(txt);
                case "PhoneNumber":
                    return x => x.PhoneNumber == txt;
                case "HomePhoneNumber":
                    return x => x.HomePhoneNumber == txt;
                case "Address":
                    return x => x.Address == txt;
                case "SubscriberId-SubscriptionId":
                    {
                        var ids = txt.Split('-');
                        var subscriberId = ids[0];
                        var subscriptionId = ids[1];

                        return x => x.Id.ToString() == subscriberId && x.Subscriptions.Any(s => s.Id.ToString() == subscriptionId);
                    };
                default:
                    return x =>
                        x.Id.ToString() == txt ||
                        x.PhoneNumber.Contains(txt) ||
                        x.HomePhoneNumber.Contains(txt) ||
                        x.FirstName.Contains(txt) ||
                        x.LastName.Contains(txt);
            }
        }

    }
}
