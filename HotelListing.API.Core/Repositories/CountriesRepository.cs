using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Countries;
using HotelListing.API.Data;
using HotelListing.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //all the specific implementations of the Countries repository like getting the hotel details
        //will be in this repository

        public async Task<GetCountryWithHotelDto> GetDetails(int id)
        {

            //project the dto to the entity query
            var country = await _context.Countries.Include(q => q.Hotels)
               .ProjectTo<GetCountryWithHotelDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }

            return country;
        }
    }
}
