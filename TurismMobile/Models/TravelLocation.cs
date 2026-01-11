using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TurismMobile.Models
{
    public class TravelLocation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele locației este obligatoriu")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Țara este obligatorie")]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        // Relații de navigare
        public List<Tour> Tours { get; set; } = new();
    }
}
