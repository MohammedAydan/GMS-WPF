using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscribersService
{
    public interface ISubscribtionService
    {
        Task<IEnumerable<Subscription>> GetSubscriptions(int page = 1, int limit = 30);
        Task<Subscription?> GetSubscription(int id);
        Task<Subscription> AddSubscription(Subscription subscription);
        Task<Subscription> UpdateSubscription(Subscription subscription);
        Task DeleteSubscription(int id);
    }
}
