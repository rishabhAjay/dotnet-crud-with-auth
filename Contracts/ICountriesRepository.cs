using HotelListing.API.Data;

namespace hotelListingAPI.Contracts
{

    //this will contain a contract of the specific repository or its specific implementations
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
