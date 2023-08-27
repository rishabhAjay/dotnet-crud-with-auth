using AutoMapper;
using HotelListing.API.Core.Models.Countries;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Core.Models.Users;
using HotelListing.API.Data.Entities;

namespace HotelListing.API.Core.Configurations.AutoMapperConfig;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        //we map the Country Object with the Create Country DTO and reverse map it to go both ways
        CreateMap<Country, CreateCountryDto>()
            .ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryWithHotelDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();

        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();

        CreateMap<User, ApiUserDto>().ReverseMap();

    }
}
