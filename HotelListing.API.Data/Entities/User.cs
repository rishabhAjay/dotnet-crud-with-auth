using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data.Entities
{

    //we are extending from IdentityUser so we inherit the properties of the built in type
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
