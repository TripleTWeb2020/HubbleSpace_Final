using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;

namespace HubbleSpace_Final.Entities
{
    public class MyDbContext: DbContext
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

        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        
    }
}
