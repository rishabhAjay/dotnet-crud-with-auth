using hotelListingAPI.data.Entities;
using hotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace hotelListingAPI.Contracts
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
