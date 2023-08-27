namespace HotelListing.API.Core.Models.Countries;

//you create DTOs to be able to model our APIs properly for its body and response types
public abstract class BaseCountryDto
{
    public string Name { get; set; }
    public string ShortName { get; set; }
}
