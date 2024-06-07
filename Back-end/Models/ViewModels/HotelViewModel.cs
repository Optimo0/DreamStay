using Backend.Models.DbModels;

namespace Backend.Models.ViewModels
{
    // Displays a list of hotels along with their city information, where both hotel and city details are needed.
    // By using a view model, it separates the concerns of the view from the database entities, allowing for more flexibility and easier testing.
    public class HotelViewModel
    {
        public City? City { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
