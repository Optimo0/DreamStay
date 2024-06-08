using Backend.Models.DbModels;

namespace Backend.Models.ViewModels
{
    // Represents the data required for booking a hotel offer
    public class BookingViewModel
    {
        public Offer Offer { get; set; }
        public int NumberOfPeople { get; set; }
        // This is calculated based on the number of people and the duration of the stay
        public decimal TotalPrice { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string UserEmail { get; set; }
    }
}
