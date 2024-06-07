using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    // Represents a hotel entity in the database
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HotelId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Range(1, 5, ErrorMessage = "Value out of range")]
        public int Rate { get; set; }

        // This is a foreign key linking the Hotel to the City it is located in.
        public int CityId { get; set; }

        //This is a navigation property that creates a relationship between the Hotel and the City entity.
        //The ForeignKey attribute specifies that this relationship is based on the CityId property.
        //The [Required] attribute ensures that a hotel must be associated with a city.
        [Required]
        [ForeignKey("CityId")]
        public virtual City? City { get; set; }

        public bool Pool { get; set; }
        public bool WiFi { get; set; }

        public Hotel(){}

        public Hotel(string? name, int rate, City? city, bool pool, bool wiFi)
        {
            Name = name;
            Rate = rate;
            City = city;
            Pool = pool;
            WiFi = wiFi;
        }

    }
}
