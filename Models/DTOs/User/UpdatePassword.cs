namespace Project_v1.Models.DTOs.User {
    public class UpdatePassword {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
