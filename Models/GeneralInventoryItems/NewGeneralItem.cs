namespace Project_v1.Models.GeneralInventoryItems {
    public class NewGeneralItem {
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public DateOnly DurationOfInventory { get; set; }
        public string Remarks { get; set; }
        public string LabId { get; set; }
    }
}
