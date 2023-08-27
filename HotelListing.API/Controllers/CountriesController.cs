using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Countries;
using HotelListing.API.Data.Entities;
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
        // you wont have to inject mapper here since we did that in the repository  
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;

        //you can inject context into any controller given the DB context being defined in program.cs as a service
        public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
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

        // GET: api/Countries?startIndex==0&PageSize=15&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {

            var pagedCountries = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        //define the decorator to protect the endpoint
        [Authorize]
        public async Task<ActionResult<GetCountryWithHotelDto>> GetCountry(int id)
        {

            var country = await _countriesRepository.GetDetails(id);
            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            //map the left side of data to the right side of the model
            //earlier implementation with mapper in controller
            //_mapper.Map(updateCountryDto, country);
            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountryDto);
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
            var country = await _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountry);

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
