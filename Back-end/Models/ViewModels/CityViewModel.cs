using Backend.Models.DbModels;

namespace Backend.Models.ViewModels
{
    // Designed to pass data about a city and its associated country to the views,
    // allowing the view to display information about both entities together.
    public class CityViewModel
    {
        public City? City { get; set; }
        public Country? Country { get; set; }
    }
}
