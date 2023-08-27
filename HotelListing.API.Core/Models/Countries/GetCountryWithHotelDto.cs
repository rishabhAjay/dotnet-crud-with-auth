using HotelListing.API.Core.Models.Hotel;

namespace HotelListing.API.Core.Models.Countries;

public class GetCountryWithHotelDto : BaseCountryDto
{
    public int Id { get; set; }
    public virtual IList<HotelDto> Hotels { get; set; }
}
