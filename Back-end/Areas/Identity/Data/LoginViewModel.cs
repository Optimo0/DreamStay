using System.ComponentModel.DataAnnotations;

namespace Backend.Areas.Identity.Data
{
    // Represents the data model for capturing user login information.
    public class LoginViewModel
    {
        // Represents the user's email address.
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Represents the user's password.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Represents whether the user wants to remain logged in.
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
