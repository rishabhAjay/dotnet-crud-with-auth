using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Countries;
using AutoMapper;

// to generate a controller from the CLI: dotnet-aspnet-codegenerator controller -m Country -dc HotelListingDbContext -api -name CountriesController -outDir Controllers/

namespace hotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        //you can inject context into any controller given the DB context being defined in program.cs as a service
        public CountriesController(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        //return an enumerable of type GetCountryDto to return only what you need to.
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var countries = await _context.Countries.ToListAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryWithHotelDto>> GetCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries
                .Include(q => q.Hotels)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                return NotFound();
            }
            var record = _mapper.Map<GetCountryWithHotelDto>(country);
            return Ok(record);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
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
            if (_context.Countries == null)
            {
                return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            }

            //mapping data with the incoming body without AutoMapper
            // var country = new Country
            // {
            //     Name = createCountry.Name,
            //     ShortName = createCountry.ShortName
            // };

            //with automapper
            var country = _mapper.Map<Country>(createCountry);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
