using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }

        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }

        public int SubscriptionTypeId { get; set; }
        public virtual SubscriptionType SubscriptionsTypes { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(30);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
