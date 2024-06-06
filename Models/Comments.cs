using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class Comment {
        [Key]
        [Column(TypeName = "varchar(10)")]
        public string CommentId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Feedback { get; set; }
    }
}
