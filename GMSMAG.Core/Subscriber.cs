using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GMSMAG.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }

        // Navigation property to Admin
        public virtual Admin Admin { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string HomePhoneNumber { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string Address { get; set; } = string.Empty;

        // A subscriber can have multiple subscriptions
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = null;
    }
}
