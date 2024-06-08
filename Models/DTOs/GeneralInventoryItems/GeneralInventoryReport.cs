namespace Project_v1.Models.DTOs.GeneralInventoryItems {
    public class GeneralInventoryReport {
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public int Duration { get; set; }
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }
        public string GeneralCategoryName { get; set; }
    }
}
