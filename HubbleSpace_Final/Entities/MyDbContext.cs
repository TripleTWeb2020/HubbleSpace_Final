using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HubbleSpace_Final.Models;

namespace HubbleSpace_Final.Entities
{
    public class MyDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Color_Product> Color_Product { get; set; }
        public DbSet<Img_Product> Img_Product { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Size> Size { get; set; }


        public MyDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<HubbleSpace_Final.Entities.DiscountUsed> DiscountUsed { get; set; }

        
    }
}
