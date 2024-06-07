using Microsoft.AspNetCore.Mvc;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    // Handles the booking of offers
    [Authorize(Roles = "User,Admin")]
    public class BookingController : Controller
    {
        private readonly DatabaseContext _context;

        public BookingController(DatabaseContext context)
        {
            _context = context;
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
        public IActionResult Book(BookingViewModel viewModel)
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

            return View("Confirm", viewModel);
        }
    }
}
