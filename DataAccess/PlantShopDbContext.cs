using Microsoft.EntityFrameworkCore;
using PlantShop.Data;


namespace PlantShop.DataAccess
{
    public class PlantShopDbContext:DbContext
    {
        public PlantShopDbContext(DbContextOptions<PlantShopDbContext> opt)
            : base(opt)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Shop> Shopes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShopPlant>()
                .HasKey(x => new
                {
                    x.ShopId,
                    x.PlantId
                });
            builder.Entity<Shop>()
               .HasData(
                new Shop
                {
                    Id = 1,
                    ShopName = " "

                });
            builder.Entity<Employee>()
                .HasData(
                new Employee
                {
                    Id=1,
                    ShopId=1,
                    FirstName="first",
                    LastName="first",
                    Password="first"
                });
            //foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}
            base.OnModelCreating(builder);
           
        }

    }
}
