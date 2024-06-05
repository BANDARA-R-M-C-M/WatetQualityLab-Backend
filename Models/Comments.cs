using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class Comment {
        [Key]
        public string CommentId { get; set; }
        public string Feedback { get; set; }
    }
}
