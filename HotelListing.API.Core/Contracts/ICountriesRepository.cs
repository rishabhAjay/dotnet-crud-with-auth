using HotelListing.API.Core.Models.Countries;
using HotelListing.API.Data.Entities;

namespace HotelListing.API.Core.Contracts
{

    //this will contain a contract of the specific repository or its specific implementations
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<GetCountryWithHotelDto> GetDetails(int id);
    }
}
