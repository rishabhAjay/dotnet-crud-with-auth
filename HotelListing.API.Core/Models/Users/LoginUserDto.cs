﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Users
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
