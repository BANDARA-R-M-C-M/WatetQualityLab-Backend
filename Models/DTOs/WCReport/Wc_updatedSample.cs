namespace Project_v1.Models.DTOs.WCReport
{
    public class Wc_updatedSample
    {
        public string YourRefNo { get; set; }
        public string StateOfChlorination { get; set; }
        public DateOnly DateOfCollection { get; set; }
        public string CatagoryOfSource { get; set; }
        public string CollectingSource { get; set; }
    }
}
