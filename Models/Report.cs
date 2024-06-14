using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class Report {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string ReportRefId { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string MyRefNo { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PresumptiveColiformCount { get; set; }

        [Column(TypeName = "date")]
        public DateOnly IssuedDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EcoliCount { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string AppearanceOfSample { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Remarks { get; set; }

        [Column(TypeName = "bit")]
        public bool Contaminated { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string MltId { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }

        [ForeignKey("Sample")]
        [Column(TypeName = "varchar(40)")]
        public string SampleId { get; set; }
        public virtual Sample Sample { get; set; }
    }
}