using Backend.Models.DbModels;

namespace Backend.Models.ViewModels
{
    // Is used in a view that displays detailed information about an offer
    // it separates the concerns of the view from the database entities, allowing for more flexibility and easier testing.
    public class OfferViewModel
    {
        public Offer? Offer { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }

        public Hotel? Hotel { get; set; }
    }
}
