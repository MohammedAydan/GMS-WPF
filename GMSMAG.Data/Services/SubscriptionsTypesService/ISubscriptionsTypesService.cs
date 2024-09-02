using GMSMAG.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscriptionsTypesService
{
    public interface ISubscriptionsTypesService
    {
        Task<IEnumerable<SubscriptionsTypes>> GetSubscriptionsTypesAsync(int page = 1, int limit = 30);
        Task<SubscriptionsTypes?> GetSubscriptionsTypeAsync(int id);
        Task<SubscriptionsTypes> AddSubscriptionsTypeAsync(SubscriptionsTypes subscriptionsType);
        Task<SubscriptionsTypes> UpdateSubscriptionsTypeAsync(SubscriptionsTypes subscriptionsType);
        Task DeleteSubscriptionsTypeAsync(int id);
    }
}