namespace Project_v1.Models.WCReport {
    public class Wc_report {
        public string ReportRefId { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public DateTime IssuedDate { get; set; }
        public string EcoliCount { get; set; }
        public string AppearanceOfSample { get; set; }
        public string PCResults { get; set; }
        public string ECResults { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
        public string SampleId { get; set; }
        public string LabId { get; set; }
    }
}