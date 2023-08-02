using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Countries;

//you create DTOs to be able to model our APIs properly for its body and response types so that we do not send excess data(overposting)
public class CreateCountryDto
{
    [Required]
    public string Name { get; set; }
    public string ShortName { get; set; }
}
