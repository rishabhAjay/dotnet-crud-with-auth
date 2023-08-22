using HotelListing.API.Data;
using hotelListingAPI.Contracts;
using Microsoft.EntityFrameworkCore;

namespace hotelListingAPI.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        //all the specific implementations of the Countries repository like getting the hotel details
        //will be in this repository

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries
                  .Include(q => q.Hotels)
                  .FirstOrDefaultAsync();
        }
    }
}
