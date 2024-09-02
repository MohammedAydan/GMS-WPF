using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Services.SubscriptionsTypesService
{
    public interface ISubscriberService
    {
        Task<IEnumerable<Subscriber>> GetSubscribers(int page = 1, int limit = 50);
        Task<Subscriber?> GetSubscriber(int id);
        Task<Subscriber?> GetLastSubscriber(int id);
        Task<Subscriber> AddSubscriber(Subscriber subscriber);
        Task<Subscriber> UpdateSubscriber(Subscriber subscriber);
        Task DeleteSubscriber(int id);
        Task<List<Subscriber>> Search(string txt,string colName = "Id",int page = 1,int limit = 50);
    }
}
