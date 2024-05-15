namespace Project_v1.Models.DTOs.GeneralInventoryItems {
    public class QRGeneral {
        public string GeneralInventoryID { get; set; }
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public int DurationOfInventory { get; set; }
        public string Remarks { get; set; }
    }
}
