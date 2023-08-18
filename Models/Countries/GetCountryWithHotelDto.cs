using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Countries;

public class GetCountryWithHotelDto : BaseCountryDto
{
    public int Id { get; set; }
    public virtual IList<HotelDto> Hotels { get; set; }
}
