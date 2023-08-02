using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Countries;

public class GetCountryWithHotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public virtual IList<HotelDto> Hotels { get; set; }
}
