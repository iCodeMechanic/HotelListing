using Microsoft.AspNetCore.Identity;

namespace HotelListing.Entity
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
