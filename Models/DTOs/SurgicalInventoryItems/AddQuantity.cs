using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class AddQuantity {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
