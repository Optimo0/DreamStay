using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    // Represents a country entity in the database
    // The Country class forms the top level of the hierarchical relationship among Country, City, and Hotel.
    // Each Country can have multiple Cities associated with it.
    // Each City can have multiple Hotels associated with it.
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public Country(){ }

        public Country(string? name)
        {
            Name = name;
        }
    }
}
