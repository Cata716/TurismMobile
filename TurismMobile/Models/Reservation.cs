using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurismMobile.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int TourId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Range(1, 50, ErrorMessage = "Numărul de persoane trebuie să fie între 1 și 50")]
        public int NumberOfPeople { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Prețul total trebuie să fie pozitiv")]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "În așteptare";

        // Relații de navigare
        public User? User { get; set; }
        public Tour? Tour { get; set; }
    }
}