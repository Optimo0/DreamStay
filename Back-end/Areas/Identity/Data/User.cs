using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Areas.Identity.Data
{
    // Represents the application user entity extending IdentityUser.
    public class User : IdentityUser
    {
        // Represents the user's first name.
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        // Represents the user's last name.
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

    }
}
