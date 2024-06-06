namespace Project_v1.Models.DTOs.WCReport
{
    public class Wc_updatedReport {
        public string MyRefNo { get; set; }
        public string AppearanceOfSample { get; set; }
        public int PresumptiveColiformCount { get; set; }
        public int EcoliCount { get; set; }
        public string Remarks { get; set; }
        public bool Contaminated { get; set; }
    }
}
