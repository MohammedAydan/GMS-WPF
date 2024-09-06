using GMSMAG.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscriptionsTypesService
{
    public interface ISubscriptionsTypesService
    {
        Task<IEnumerable<SubscriptionType>> GetSubscriptionsTypesAsync(int page = 1, int limit = 30);
        Task<SubscriptionType?> GetSubscriptionsTypeAsync(int id);
        Task<SubscriptionType> AddSubscriptionsTypeAsync(SubscriptionType subscriptionsType);
        Task<SubscriptionType> UpdateSubscriptionsTypeAsync(SubscriptionType subscriptionsType);
        Task DeleteSubscriptionsTypeAsync(int id);
    }
}