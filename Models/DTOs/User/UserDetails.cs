using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.User {
    public class UserDetails {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters Only")]
        [StringLength(30, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string? UserName { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Content no exeed 250 characters")]
        [DataType(DataType.Text)]
        public string? ImageUrl { get; set; }
    }
}
