using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurismMobile.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Format email invalid")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola trebuie să aibă minim 6 caractere")]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        [Phone(ErrorMessage = "Format număr de telefon invalid")]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string Role { get; set; } = "User";

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
       
        public List<Reservation> Reservations { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }
}