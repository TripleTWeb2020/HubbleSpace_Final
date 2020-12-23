using Microsoft.EntityFrameworkCore;

namespace HubbleSpace_Final.Entities
{
    public class MyDbContext: DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Product> Products { get; set; }

        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
