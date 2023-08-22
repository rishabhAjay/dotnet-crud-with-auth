using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Countries;
using HotelListing.API.Models.Hotel;
using hotelListingAPI.data.Entities;
using hotelListingAPI.Models.Users;

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
        CreateMap<Country, UpdateCountryDto>().ReverseMap();

        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();

        CreateMap<User, ApiUserDto>().ReverseMap();

    }
}
