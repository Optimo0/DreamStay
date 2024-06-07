using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    // Represents a city entity in the database
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        // This is a foreign key linking the City to the Country it is located in.
        public int CountryId { get; set; }

        // This is a navigation property that creates a relationship between the City and the Country entity.
        // The ForeignKey attribute specifies that this relationship is based on the CountryId property.
        // The [Required] attribute ensures that a city must be associated with a country.
        [Required]
        [ForeignKey("CountryId")]
        public virtual Country? Country{ get; set; }

        public City(){}

        public City(string? name, Country? country)
        {
            Name = name;
            Country = country;
        }
    }
}
