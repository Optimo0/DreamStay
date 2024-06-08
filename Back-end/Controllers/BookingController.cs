using Microsoft.AspNetCore.Mvc;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Backend.Controllers
{
    // Handles the booking of offers
    [Authorize(Roles = "User,Admin")]
    public class BookingController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays the booking form for a specific offer
        [HttpGet]
        public IActionResult Book(int offerId)
        {
            var offer = _context.Offer
                                .Include(o => o.Hotel)
                                .FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null)
            {
                return NotFound();
            }

            // Create a new BookingViewModel with default values
            var viewModel = new BookingViewModel
            {
                Offer = offer,
                NumberOfPeople = 1,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(7),
                TotalPrice = offer.Price
            };
            return View(viewModel);
        }

        // Handles the submission of the booking form
        [HttpPost]
        public async Task<IActionResult> Book(BookingViewModel viewModel)
        {
            var offer = _context.Offer
                                .Include(o => o.Hotel)
                                .FirstOrDefault(o => o.OfferId == viewModel.Offer.OfferId);
            if (offer == null)
            {
                return NotFound();
            }

            viewModel.Offer = offer;

            // Calculate the number of days between the selected dates
            var numberOfDays = (viewModel.DateTo - viewModel.DateFrom).Days;
            if (numberOfDays <= 0)
            {
                ModelState.AddModelError("", "Invalid date range.");
                return View(viewModel);
            }

            // Calculate the price per day
            var pricePerDay = offer.Price / 7;

            // Calculate the total price based on the number of days and number of people
            viewModel.TotalPrice = pricePerDay * numberOfDays * viewModel.NumberOfPeople;

            // Get the logged-in user's email
            var user = await _userManager.GetUserAsync(User);
            var userEmail = user?.Email;
            viewModel.UserEmail = userEmail;

            // Save the booking information to the database
            var booking = new Booking
            {
                OfferId = viewModel.Offer.OfferId,
                UserEmail = viewModel.UserEmail,
                NumberOfPeople = viewModel.NumberOfPeople,
                TotalPrice = viewModel.TotalPrice,
                DateFrom = viewModel.DateFrom,
                DateTo = viewModel.DateTo
            };

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return View("Confirm", viewModel);
        }
    }
}
