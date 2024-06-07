using Microsoft.AspNetCore.Mvc;
using Backend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    // Controller for account-related actions
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // Constructor to initialize the UserManager and SignInManager
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Action method for handling user registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new user with the provided email and password
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                // If user creation is successful
                if (result.Succeeded)
                {
                    // Assign the "User" role to the newly registered user
                    await _userManager.AddToRoleAsync(user, "User");
                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // Redirect to the home page
                    return RedirectToAction("Index", "Home");
                }
                // If there are errors during user creation, add them to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed; redisplay the form
            return View(model);
        }
    }
}
