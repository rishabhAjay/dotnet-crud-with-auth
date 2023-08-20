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
            modelBuilder
                .Entity<Country>()
                .HasData(
                    new Country
                    {
                        Id = 1,
                        Name = "Jamaica",
                        ShortName = "JM"
                    },
                    new Country
                    {
                        Id = 2,
                        Name = "Bahamas",
                        ShortName = "BS"
                    },
                    new Country
                    {
                        Id = 3,
                        Name = "Cayman Island",
                        ShortName = "CI"
                    }
                );

            modelBuilder
                .Entity<Hotel>()
                .HasData(
                    new Hotel
                    {
                        Id = 1,
                        Name = "Sandals Resort and Spa",
                        Address = "Negril",
                        CountryId = 1,
                        Rating = 4.5
                    },
                    new Hotel
                    {
                        Id = 2,
                        Name = "Comfort Suites",
                        Address = "George Town",
                        CountryId = 3,
                        Rating = 4.3
                    },
                    new Hotel
                    {
                        Id = 3,
                        Name = "Grand Palldium",
                        Address = "Nassua",
                        CountryId = 2,
                        Rating = 4
                    }
                );
        }
    }
}
