namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class ItemIssuingReport {
        public string ItemName { get; set; }
        public string SurgicalCategory { get; set; }
        public int InitialQuantity { get; set; }
        public int IssuedInMonth { get; set; }
        public int AddedInMonth { get; set; }
        public int RemainingQuantity { get; set; }
    }
}
