using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TurismMobile.Models
{
    public class Tour
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000, ErrorMessage = "Prețul trebuie să fie între 1 și 100000")]
        public decimal Price { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Foreign Key
        [Required]
        public int LocationId { get; set; }

        // Relații de navigare
        public TravelLocation? Location { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }
}