using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurismMobile.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int TourId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Evaluarea trebuie să fie între 1 și 5 stele")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comentariul nu poate depăși 1000 de caractere")]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsHelpful { get; set; } = false;

        // Relații de navigare
        public User? User { get; set; }
        public Tour? Tour { get; set; }
    }
}
