namespace Project_v1.Models.DTOs.WCReport
{
    public class Wc_updatedReport {
        public string MyRefNo { get; set; }
        public string AppearanceOfSample { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public string EcoliCount { get; set; }
        public string Remarks { get; set; }
        public bool Contaminated { get; set; }
    }
}
