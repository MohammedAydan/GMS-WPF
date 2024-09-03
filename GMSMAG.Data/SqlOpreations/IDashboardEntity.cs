using GMSMAG.Core;
using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Data.SqlOpreations
{
    public interface IDashboardEntity
    {
        Task<int> GetSubscribersCountAsync();
        Task<int> GetSubscriptionsCountAsync();
        Task<int> GetSubscriptionsTypesCountAsync();

        Task<List<SubscriberInfo>> GetExpiredSubscribersAsync(int Page = 1,int Limit = 50);
        //Task<ICollection<Subscriber>> GetExpiredSubscribers();
    }
}
