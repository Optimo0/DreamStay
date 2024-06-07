namespace Backend.Models.ViewModels
{
    // Serves as a data transfer object between the view and the controller when creating new hotel records
    public class CreateHotelViewModel
    {
        public int HotelId { get; set; }
        public string? Name { get; set; }
        public int Rate { get; set; }
        public string? City { get; set; }
        public bool WiFi { get; set; }
        public bool Pool { get; set; }
    }
}
