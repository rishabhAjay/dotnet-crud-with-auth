using HotelListing.API.Data;
using hotelListingAPI.Contracts;

namespace hotelListingAPI.Repositories
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
        }
    }
}
