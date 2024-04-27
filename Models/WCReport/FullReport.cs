namespace Project_v1.Models.WCReport {
    public class FullReport {
        public string SampleId { get; set; }
        public string StateOfChlorination { get; set; }
        public DateTime DateOfCollection { get; set; }
        public string CatagoryOfSource { get; set; }
        public string CollectingSource { get; set; }
        public DateTime AnalyzedDate { get; set; }
        public string phiAreaName { get; set; }
        public string ReportRefId { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public DateTime IssuedDate { get; set; }
        public string EcoliCount { get; set; }
        public string AppearanceOfSample { get; set; }
        public string Results { get; set; }
        public string Remarks { get; set; }
    }
}
