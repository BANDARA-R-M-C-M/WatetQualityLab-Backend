using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class Report {
        [Key]
        public string ReportRefId { get; set; }
        public string PresumptiveColiformCount { get; set; }
        public DateTime IssuedDate { get; set; }
        public string EcoliCount { get; set; }
        public string AppearanceOfSample { get; set; }
        public string Results { get; set; }
        public string Remarks { get; set; }
        public string SampleRefId { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual Sample Sample { get; set; }
    }
}
