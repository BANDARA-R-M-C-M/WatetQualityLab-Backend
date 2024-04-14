using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class Sample {
        [Key]
        public string SampleRefId { get; set; }
        public string StateOfChlorination { get; set; }
        public DateOnly DateOfCollection { get; set; }
        public string CatagoryOfUse { get; set; }
        public string CollectingSource { get; set; }
        public DateOnly AnalyzedDate { get; set; }
        public string Phi_Area { get; set; }
        public bool Acceptance { get; set; }
        public string PHIAreaId { get; set; }
        public virtual PHIArea PHIArea { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
