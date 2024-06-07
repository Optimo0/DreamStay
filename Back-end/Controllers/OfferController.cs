using Backend.Models;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public class OfferController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> cities;
        private readonly IQueryable<string> hotels;
        private readonly IQueryable<string> countries;
        private readonly string[] orders;

        public OfferController(DatabaseContext context)
        {
            _context = context;

            // Initialize queryable variables for cities, hotels, countries, and sorting orders
            cities = from c in _context.City
                     orderby c.Name
                     select c.Name;

            hotels = from h in _context.Hotel
                     orderby h.Name
                     select h.Name;

            countries = from c in _context.Country
                        orderby c.Name
                        select c.Name;

            orders = new string[] { "Ascending", "Descending" };
        }

        // Action method to display the list of offers
        [AllowAnonymous]
        public IActionResult Index(string countrySearch, string sortOrder, string cityFrom, string cityTo, int hotelId, int page = 1)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));


            var offers = from o in _context.Offer
                         select o;

            switch (sortOrder)
            {
                case "Descending":
                    offers = offers.OrderByDescending(s => s.Price);
                    break;
                case "Ascending":
                    offers = offers.OrderBy(s => s.Price);
                    break;
            }

            // Filtering offers based on search criteria
            List<OfferViewModel> offerViewModels = new List<OfferViewModel>();
            foreach (var offer in offers)
            {
                var hotel = _context.Hotel.Find(offer.HotelId);

                if (hotel == null)
                    continue;

                var city = _context.City.Find(hotel.CityId);

                if (city == null)
                    continue;

                var cityDep = _context.City.Find(offer.DepartureCityId);

                if (cityDep == null)
                    continue;

                var country = _context.Country.FirstOrDefault(c => c.CountryId == city.CountryId);

                if (country == null)
                    continue;

                if (country.Name == countrySearch || string.IsNullOrEmpty(countrySearch))
                {
                    if (cityDep.Name == cityFrom || string.IsNullOrEmpty(cityFrom))
                    {
                        if (city.Name == cityTo || string.IsNullOrEmpty(cityTo))
                        {
                            if (hotel.HotelId == hotelId || hotelId == 0)
                            {
                                offerViewModels.Add(new OfferViewModel()
                                {
                                    Country = country,
                                    Offer = offer,
                                    Hotel = hotel,
                                    City = city
                                });
                            }
                        }
                    }
                }
            }

            // Setting up ViewBag variables for dropdowns and pagination
            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            ViewBag.Countries = new SelectList(countries, (string.IsNullOrEmpty(countrySearch) || !countries.Contains(countrySearch)) ? "All" : countrySearch);
            ViewBag.CitiesFrom = new SelectList(cities, (string.IsNullOrEmpty(cityFrom) || !cities.Contains(cityFrom)) ? "All" : cityFrom);
            ViewBag.CitiesTo = new SelectList(cities, (string.IsNullOrEmpty(cityTo) || !cities.Contains(cityTo)) ? "All" : cityTo);

            // Cities only from selected country
            if (!string.IsNullOrEmpty(countrySearch))
            {
                var country = _context.Country.FirstOrDefault(c => c.Name == countrySearch);

                if (country != null)
                {
                    var cityQuery = from c in _context.City
                                    join cn in _context.Country on c.CountryId equals cn.CountryId
                                    where c.CountryId == country.CountryId
                                    orderby c.Name
                                    select c.Name;

                    ViewBag.CitiesTo = new SelectList(cityQuery, (string.IsNullOrEmpty(cityTo) || !cities.Contains(cityTo)) ? "All" : cityTo);
                }
            }

            // Pagination logic
            offerViewModels = offerViewModels.Skip((page - 1) * 10).ToList();
            ViewBag.DisabledRight = false;
            ViewBag.DisabledLeft = false;

            if (offerViewModels.Count == 0)
            {
                ViewBag.DisabledRight = true;
                ViewBag.DisabledLeft = true;
            }

            if (offerViewModels.Count < 11)
                ViewBag.DisabledRight = true;

            if (page == 1)
                ViewBag.DisabledLeft = true;

            offerViewModels = offerViewModels.Take(10).ToList();

            return View(offerViewModels);
        }


        // Action method to display the create offer form
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            ViewBag.Cities = new SelectList(cities);
            ViewBag.Hotels = new SelectList(hotels);

            var model = new CreateOfferViewModel();
            return View(model);
        }

        // Action method to handle the submission of the create offer form
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateOfferViewModel offerModel)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));


            var hotel = _context.Hotel.FirstOrDefault(Hotel => Hotel.Name == offerModel.Hotel);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Incorrect hotel"));

            var city = _context.City.FirstOrDefault(City => City.Name == offerModel.DepartureCity);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));


            var offer = new Offer
            {
                HotelId = hotel.HotelId,
                DepartureCityId = city.CityId,
                Price = (decimal)offerModel.Price,
                DateTo = offerModel.DateTo,
                DateFrom = offerModel.DateFrom,
                FullBoard = offerModel.FullBoard
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(offer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - offer was not created"));
                }
            }

            ViewBag.Cities = new SelectList(cities);
            ViewBag.Hotels = new SelectList(hotels);
            return View(offerModel);

        }

        // Action method to display the edit offer form
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with routing url"));

            var offer = _context.Offer.Find(id);

            if (offer == null)
                return View("Error", new ErrorViewModel("Offer not found"));

            ViewBag.Cities = new SelectList(cities);
            ViewBag.Hotels = new SelectList(hotels);
            ViewBag.ID = id;

            var viewModel = GenerateCreateOfferViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Offer);

            return View(viewModel);
        }

        // Action method to handle the submission of the edit offer form
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateOfferViewModel newModel)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var offer = _context.Offer.Find(newModel.OfferId);

            if (offer == null)
                return View("Error", new ErrorViewModel("Offer not found"));

            var hotel = _context.Hotel.FirstOrDefault(Hotel => Hotel.Name == newModel.Hotel);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Incorrect hotel"));

            var city = _context.City.FirstOrDefault(City => City.Name == newModel.DepartureCity);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));


            offer.Price = (decimal)newModel.Price;
            offer.DateTo = newModel.DateTo;
            offer.DateFrom = newModel.DateFrom;
            offer.FullBoard = newModel.FullBoard;
            offer.DepartureCityId = city.CityId;
            offer.HotelId = hotel.HotelId;


            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - offer was not edited"));
                }
            }

            ViewBag.ID = newModel.OfferId;
            ViewBag.Cities = new SelectList(cities);
            ViewBag.Hotels = new SelectList(hotels);

            return View(GenerateCreateOfferViewModel(newModel.OfferId));
        }

        // Action method to display the details of an offer
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with routing url"));

            var offer = _context.Offer.FirstOrDefault(of => of.OfferId == id);

            if (offer == null)
                return View("Error", new ErrorViewModel("Offer not found"));

            var viewModel = GenerateOfferViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Offer);

            return View(viewModel);

        }

        // Action method to display the delete confirmation page
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var offer = _context.Offer.Find(id);

            if (offer == null)
                return View("Error", new ErrorViewModel("Offer not found"));

            var viewModel = GenerateOfferViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Offer);

            return View(viewModel);
        }

        // Action method to handle the deletion of an offer
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var offer = _context.Offer.Find(id);

            if (offer == null)
                return View("Error", new ErrorViewModel("Offer not found"));

            try
            {
                _context.Offer.Remove(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - offer was not deleted"));
            }
        }

        // Method to generate an OfferViewModel object
        [ApiExplorerSettings(IgnoreApi = true)]
        public OfferViewModel? GenerateOfferViewModel(int offerId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var offer = _context.Offer.Find(offerId);

            if (offer == null)
                return null;

            var hotel = _context.Hotel.Find(offer.HotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var country = _context.Country.Find(city.CountryId);

            var viewModel = new OfferViewModel()
            {
                Country = country,
                Offer = offer,
                Hotel = hotel,
                City = city
            };

            return viewModel;
        }

        // Method to generate a CreateOfferViewModel object
        [ApiExplorerSettings(IgnoreApi = true)]
        public CreateOfferViewModel? GenerateCreateOfferViewModel(int offerId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var offer = _context.Offer.Find(offerId);

            if (offer == null)
                return null;

            var hotel = _context.Hotel.Find(offer.HotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var cityDep = _context.City.Find(offer.DepartureCityId);

            if (cityDep == null)
                return null;

            var viewModel = new CreateOfferViewModel()
            {
                OfferId = offerId,
                Hotel = hotel.Name,
                Price = (double)offer.Price,
                DepartureCity = cityDep.Name,
                DateFrom = offer.DateFrom,
                DateTo = offer.DateTo,
                FullBoard = offer.FullBoard
            };

            return viewModel;
        }
    }
}