namespace Project_v1.Models.DTOs.WCReport
{
    public class Wc_report
    {
        public string MyRefNo { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public DateOnly AnalyzedDate { get; set; }
        public string EcoliCount { get; set; }
        public string AppearanceOfSample { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
        public string SampleId { get; set; }
        public string LabId { get; set; }
    }
}