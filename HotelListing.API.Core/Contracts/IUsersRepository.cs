using HotelListing.API.Core.Models.Users;
using HotelListing.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.Contracts
{
    public interface IUsersRepository
    {

        //register the methods in this repository
        Task<IEnumerable<IdentityError>> registerUser(ApiUserDto userDto);
        Task<LoginResponseDto> loginUser(LoginUserDto userDto);

        Task<string> generateToken(User user);

        Task<string> createRefreshToken(User user);
        Task<LoginResponseDto> verifyRefreshToken(LoginResponseDto userDto);

    }
}
