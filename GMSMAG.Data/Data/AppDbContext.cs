using GMSMAG.Models;
using Microsoft.EntityFrameworkCore;

namespace GMSMAG.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost; database=gms; user=root; pwd=Aa7498459;");
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<SubscriptionsTypes> SubscriptionsTypes { get; set; }
    }
}
