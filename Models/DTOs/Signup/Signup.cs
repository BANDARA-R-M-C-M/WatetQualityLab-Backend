using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.Signup {
    public class Signup {
        [Required]
        [RegularExpression(@"^\d{12}$|^\d{9}[vV]$", ErrorMessage = "Invalid identity card format")]
        public string Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password should be between 8 - 12 character long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(Mlt|Phi|MOH_Supervisor|Admin)$", ErrorMessage = "Invalid Role")]
        public string Role { get; set; }
    }
}