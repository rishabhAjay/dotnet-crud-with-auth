using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Countries;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Configurations.AutoMapperConfig;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        //we map the Country Object with the Create Country DTO and reverse map it to go both ways
        CreateMap<Country, CreateCountryDto>()
            .ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryWithHotelDto>().ReverseMap();

        CreateMap<Hotel, HotelDto>().ReverseMap();
    }
}
