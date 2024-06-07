using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models.DbModels;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Controllers
{
    // Handles country-related operations
    public class CountryController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly string[] orders;


        public CountryController(DatabaseContext context)
        {
            _context = context;
            orders = new string[] { "Ascending", "Descending" };

        }

        // Action method for displaying the list of countries
        [AllowAnonymous]
        public IActionResult Index(string sortOrder, int page = 1)
        {
            // Check if any required database tables are null
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            // Fetch all countries from the database
            var countries = from c in _context.Country
                         select c;

            // Return the view with the list of countries
            switch (sortOrder)
            {
                case "Descending":
                    countries = countries.OrderByDescending(s => s.Name);
                    break;
                case "Ascending":
                    countries = countries.OrderBy(s => s.Name);
                    break;
            }

            // Set ViewBag properties for sorting and pagination
            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            var countries_ = countries.Skip((page - 1) * 20).ToList();
            ViewBag.DisabledRight = false;
            ViewBag.DisabledLeft = false;

            if (countries.ToList().Count == 0)
            {
                ViewBag.DisabledRight = true;
                ViewBag.DisabledLeft = true;
            }

            if (countries.ToList().Count < 21)
                ViewBag.DisabledRight = true;

            if (page == 1)
                ViewBag.DisabledLeft = true;

            countries_ = countries_.Take(20).ToList();

            // Return the view with the list of countries
            return View(countries_);
        }

        // Action method for displaying the country creation form
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            // Return the view for creating a country
            return View();
        }

        // Action method for handling the creation of a new country
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Country country)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            if (ModelState.IsValid)
            {
                try
                {
                    // Add the new country to the database and save changes
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - country was not created"));
                }
            }

            // Return the view with the country model
            return View(country);
        }

        // Action method for displaying the country editing form
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            // Find the country by ID
            var country = await _context.Country.FindAsync(id);

            // Check if the country was found
            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));

            // Return the view with the country model
            ViewBag.ID = id;
            return View(country);
        }

        // Action method for handling the editing of a country
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Country country)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country_ = _context.Country.Find(country.CountryId);

            if (country_ == null)
                return View("Error", new ErrorViewModel("Country not found"));

            // Update the country name
            country_.Name = country.Name;

            if (ModelState.IsValid)
            {
                try
                {
                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - country was not edited"));
                }
            }

            // Return the view with the country model
            return View(country);
        }

        // Action method for displaying the country deletion confirmation form
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country = await _context.Country.FirstOrDefaultAsync(m => m.CountryId == id);

            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));

            return View(country);
        }

        // Action method for handling the deletion of a country
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country = await _context.Country.FindAsync(id);

            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));


            try
            {
                // Remove the country from the database and save changes
                _context.Country.Remove(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - country was not deleted"));
            }
        }

    }
}
