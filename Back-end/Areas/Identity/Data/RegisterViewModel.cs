using System.ComponentModel.DataAnnotations;

namespace Backend.Areas.Identity.Data
{
    // Represents the data model for capturing user registration information.
    public class RegisterViewModel
    {
        // Represents the user's email address.
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Represents the user's password.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Represents the confirmation of the user's password.
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
    }
}
