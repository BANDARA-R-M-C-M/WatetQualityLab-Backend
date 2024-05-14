namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class IssueItem {
        public string ItemId { get; set; }
        public int Quantity { get; set;}
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }
    }
}
