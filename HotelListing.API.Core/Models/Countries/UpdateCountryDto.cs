namespace HotelListing.API.Core.Models.Countries;

//you create DTOs to be able to model our APIs properly for its body and response types
public class UpdateCountryDto : BaseCountryDto
{
    public int Id { get; set; }
}
