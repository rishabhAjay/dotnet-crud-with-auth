using System.ComponentModel.DataAnnotations;

namespace hotelListingAPI.Models.Users
{
    public class ApiUserDto : LoginUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


    }
}
