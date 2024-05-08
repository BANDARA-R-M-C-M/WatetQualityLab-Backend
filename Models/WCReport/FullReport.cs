namespace Project_v1.Models.WCReport {
    public class FullReport {
        public string YourRefNo { get; set; }   //sampleId
        public string StateOfChlorination { get; set; }
        public DateTime DateOfCollection { get; set; }
        public string CollectingSource { get; set; }
        public DateTime AnalyzedDate { get; set; }
        public string MyRefNo { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public DateTime IssuedDate { get; set; }
        public string LabName { get; set; }
        public string LabLocation { get; set; }
        public string LabTelephone { get; set; }
        public string EcoliCount { get; set; }
        public string AppearanceOfSample { get; set; }
        public string Results { get; set; }
    }
}
