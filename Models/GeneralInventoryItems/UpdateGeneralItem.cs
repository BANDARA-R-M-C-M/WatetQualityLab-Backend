namespace Project_v1.Models.GeneralInventoryItems {
    public class UpdateGeneralItem {
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }
        public string GeneralCategoryID { get; set; }
    }
}
