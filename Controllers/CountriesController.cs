using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Countries;
using hotelListingAPI.Contracts;
using hotelListingAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// to generate a controller from the CLI: dotnet-aspnet-codegenerator controller -m Country -dc HotelListingDbContext -api -name CountriesController -outDir Controllers/

namespace hotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;

        //you can inject context into any controller given the DB context being defined in program.cs as a service
        public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        //return an enumerable of type GetCountryDto to return only what you need to.
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            try
            {
                var countries = await _countriesRepository.GetAllAsync();
                var records = _mapper.Map<List<GetCountryDto>>(countries);
                return Ok(records);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(nameof(GetCountries));
            }
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        //define the decorator to protect the endpoint
        [Authorize]
        public async Task<ActionResult<GetCountryWithHotelDto>> GetCountry(int id)
        {

            var country = await _countriesRepository.GetDetails(Convert.ToInt32(id));
            Console.WriteLine("---->>>>" + country);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }
            var record = _mapper.Map<GetCountryWithHotelDto>(country);
            return Ok(record);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest();
            }

            // _context.Entry(country).State = EntityState.Modified;

            //find the record
            var country = await _countriesRepository.GetAsync(id);

            //map the left side of data to the right side of the model
            _mapper.Map(updateCountryDto, country);
            try
            {
                await _countriesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    throw new NotFoundException(nameof(PutCountry), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //the function names that you see(like GetCountry, PostCountry) are actions
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {


            //mapping data with the incoming body without AutoMapper
            // var country = new Country
            // {
            //     Name = createCountry.Name,
            //     ShortName = createCountry.ShortName
            // };

            //with automapper
            var country = _mapper.Map<Country>(createCountry);
            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        //specify the roles that you want to restrict these actions to. this returns 403
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {

            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
            }

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
