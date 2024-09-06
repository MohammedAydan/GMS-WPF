using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GMSMAG.Models
{
    public class SubscriptionType
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }

        [Required]
        [MaxLength(100)]
        public string SubscriptionName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string SubscriptionDescription { get; set; }

        // Using a string to store JSON array, if needed consider using a complex type or JsonDocument/JsonElement
        [Required]
        public string SubscriptionFeatures { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // create field for show SubscriptionName price
        public string SubscriptionNamePrice
        {
            get
            {
                return SubscriptionName + " - " + Price +" EGP";
            }
        }
    }
}
