using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers
{
    // Handles hotel-related operations
    public class HotelController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> cities;
        private readonly string[] orders;

        // Constructor to initialize the controller with the database context
        public HotelController(DatabaseContext context)
        {
            _context = context;

            // Query retrieves city names orderes alphabetically
            cities = from c in _context.City
                     orderby c.Name
                     select c.Name;

            // Defines sort order options
            orders = new string[] { "Ascending", "Descending" };
        }

        // Displays the list of hotels, supports search and sorting
        [AllowAnonymous]
        public IActionResult Index(string citySearch, string sortOrder, int page = 1)
        {
            // Checks if essential data in the database is null, and displays an error if so
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            // Retrieve hotels from the database
            var hotels = from h in _context.Hotel
                         select h;

            // Applies sorting based on user selection
            switch (sortOrder)
            {
                case "Descending":
                    hotels = hotels.OrderByDescending(s => s.Rate);
                    break;
                case "Ascending":
                    hotels = hotels.OrderBy(s => s.Rate);
                    break;
            }
            // Create a list of HotelViewModel to hold hotel and city details
            var viewModels = new List<HotelViewModel>();

            // Filters hotels by city search criteria and creates view models
            foreach (Hotel hotel in hotels)
            {
                // Retrieve the city associated with the hotel
                var city = _context.City.Find(hotel.CityId);

                // Skip if city is null
                if (city == null)
                    continue;

                // Add the hotel and city details to the view model list if city matches the search criteria
                if (city.Name == citySearch || string.IsNullOrEmpty(citySearch))
                {
                    viewModels.Add(new HotelViewModel()
                    {
                        Hotel = hotel,
                        City = city
                    });
                }
            }

            // Sets up dropdown lists for sorting orders and city selection
            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            ViewBag.Cities = new SelectList(cities, (string.IsNullOrEmpty(citySearch) || !cities.Contains(citySearch)) ? "All" : citySearch);

            // Pagination logic
            viewModels = viewModels.Skip((page - 1) * 10).ToList();

            ViewBag.DisabledRight = false;
            ViewBag.DisabledLeft = false;

            if (viewModels.Count == 0)
            {
                ViewBag.DisabledRight = true;
                ViewBag.DisabledLeft = true;
            }

            if (viewModels.Count < 11)
                ViewBag.DisabledRight = true;

            if (page == 1)
                ViewBag.DisabledLeft = true;

            viewModels = viewModels.Take(10).ToList();

            return View(viewModels);
        }

        // Displays details of a specific hotel
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var viewModel = new HotelViewModel()
            {
                Hotel = hotel,
                City = city
            };

            return View(viewModel);
        }

        // Displays the form to create a new hotel
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            // Prepare the city selection dropdown list and render the create view
            ViewBag.Cities = new SelectList(cities);
            var viewModel = new CreateHotelViewModel();
            return View(viewModel);
        }

        // Handles the submission of the form to create a new hotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateHotelViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.FirstOrDefault(c => c.Name == model.City);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));

            var hotel = new Hotel()
            {
                Name = model.Name,
                CityId = city.CityId,
                WiFi = model.WiFi,
                Pool = model.Pool,
                Rate = model.Rate
            };

            // Save the new hotel to the database if model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(hotel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - hotel was not created"));
                }
            }

            // If model state is not valid, re-render the create view with validation errors
            ViewBag.Cities = new SelectList(cities);
            return View(model);
        }

        // Displays the form to edit an existing hotel
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = await _context.Hotel.FindAsync(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            // Prepare the city selection dropdown list and render the edit view with hotel details
            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = id;

            var viewModel = GenerateCreateHotelViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Hotel);

            return View(viewModel);
        }

        // Handles the submission of the form to edit an existing hotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateHotelViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.FirstOrDefault(c => c.Name == model.City);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));

            var hotel = _context.Hotel.Find(model.HotelId);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Incorrect hotel"));

            hotel.Name = model.Name;
            hotel.CityId = city.CityId;
            hotel.Rate = model.Rate;
            hotel.Pool = model.Pool;
            hotel.WiFi = model.WiFi;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - hotel was not edited"));
                }
            }

            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = hotel.HotelId;

            return View(model.HotelId);
        }

        // Displays the form to delete a hotel
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var viewModel = GenerateHotelViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Hotel);

            return View(viewModel);
        }

        // Handles the submission of the form to delete a hotel
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            try
            {
                // Remove the hotel from the database and save changes
                _context.Hotel.Remove(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - hotel was not deleted"));
            }
        }

        // Generates a view model for displaying hotel details
        public HotelViewModel? GenerateHotelViewModel(int hotelId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var hotel = _context.Hotel.Find(hotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var viewModel = new HotelViewModel()
            {
                City = city,
                Hotel = hotel,
            };
            return viewModel;
        }

        // Generates and Creates a view model for displaying hotel details
        public CreateHotelViewModel? GenerateCreateHotelViewModel(int hotelId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var hotel = _context.Hotel.Find(hotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var viewModel = new CreateHotelViewModel()
            {
                HotelId = hotelId,
                Name = hotel.Name,
                City = city.Name,
                Rate = hotel.Rate,
                WiFi = hotel.WiFi,
                Pool = hotel.Pool
            };

            return viewModel;
        }
    }
}

