using Backend.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DbModels
{
    // Represents users who booked resorts in the database
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
