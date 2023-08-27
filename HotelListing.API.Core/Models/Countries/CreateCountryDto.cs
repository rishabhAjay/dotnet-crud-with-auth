using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Countries;

//you create DTOs to be able to model our APIs properly for its body and response types so that we do not send excess data(overposting)
public class CreateCountryDto : BaseCountryDto
{
    [Required]
    public string Name { get; set; }
}
