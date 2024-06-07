using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.Login {
    public class Login {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters Only")]
        [StringLength(30, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "Password should be between 8 - 12 character long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}