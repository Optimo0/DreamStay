using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    // Represents hotel offer.
    // establishes relationships with other entities, allowing for efficient data retrieval and manipulation through Entity Framework.
    public class Offer
    {
        public Offer(Hotel? hotel, decimal price, DateTime dateFrom, DateTime dateTo, City?
            departureCity, bool fullBoard)
        {
            Hotel = hotel;
            Price = price;
            DateFrom = dateFrom;
            DateTo = dateTo;
            DepartureCity = departureCity;
            FullBoard = fullBoard;
        }
        public Offer() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfferId { get; set; }

        public int HotelId { get; set; }

        [Required]
        [ForeignKey("HotelId")]
        public virtual Hotel? Hotel { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        public int DepartureCityId { get; set; }

        [Required]
        [ForeignKey("DepartureCityId")]
        public virtual City? DepartureCity { get; set; }

        public bool FullBoard { get; set; }

    }
}
