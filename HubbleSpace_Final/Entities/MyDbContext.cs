using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HubbleSpace_Final.Entities
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Color_Product> Color_Product { get; set; }
        public DbSet<Img_Product> Img_Product { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<NotificationPusher> Notifications{ get; set; }
        public DbSet<EmailSubscription> EmailSubscription { get; set; }


        public MyDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<HubbleSpace_Final.Entities.DiscountUsed> DiscountUsed { get; set; }


    }
}
