namespace Backend.Models.ViewModels
{
    // Serves as a data transfer object between the view and the controller when creating new city records.
    public class CreateCityViewModel
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
    }
}
