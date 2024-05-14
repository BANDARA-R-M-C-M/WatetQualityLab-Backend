namespace Project_v1.Models.DTOs.GeneralInventoryItems
{
    public class NewGeneralItem
    {
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }
        public string GeneralCategoryID { get; set; }
        public string LabId { get; set; }
    }
}
