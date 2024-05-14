namespace Project_v1.Models.SurgicalInventoryItems {
    public class UpdateSurgicalItem {
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }
        public string SurgicalCategoryID { get; set; }
    }
}
