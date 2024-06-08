using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;


namespace Backend.Models.DbModels
{
    // It defines the schema of the database and configures entity behaviors and relationships.
    // By inheriting from IdentityDbContext, it also integrates ASP.NET Core Identity functionalities,
    // providing a complete setup for user authentication and authorization along with the application's data management.
    public class DatabaseContext : IdentityDbContext<IdentityUser>
    {
        private readonly DbContextOptions _options;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            _options = options;
        }

        // DbSets representing tables in the database
        public DbSet<Country>? Country { get; set; }
        public DbSet<City>? City { get; set; }
        public DbSet<Hotel>? Hotel { get; set; }
        public DbSet<Offer>? Offer { get; set; }
        public DbSet<Booking>? Booking { get; set; }


        // Method for configuring entity properties and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Offer>().Property(o => o.Price).HasConversion<double>();

            modelBuilder.Entity<Offer>()
           .Property(o => o.DateFrom)
           .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Offer>()
           .Property(o => o.DateTo)
           .HasDefaultValueSql("getdate()");
        }
    }

}

