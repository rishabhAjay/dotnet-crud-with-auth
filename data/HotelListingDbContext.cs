using hotelListingAPI.data.Configuration;
using hotelListingAPI.data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{

    //we inherit from IdentityDbContext now that we have added Identity Core
    //we have also added to the context the reference to the User model
    public class HotelListingDbContext : IdentityDbContext<User>
    //the db context is where all your tables and models go
    {
        public HotelListingDbContext(DbContextOptions options)
            : base(options) { }

        //dotnet-ef migrations add migration-name
        //dotnet-ef database update to run a migration
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        //we define the seeders for the two tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seperate the seeders and other table change queries in a seperate file structure.
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }
    }
}
