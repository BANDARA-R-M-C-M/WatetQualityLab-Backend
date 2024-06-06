using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class Sample {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string SampleId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string YourRefNo { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string StateOfChlorination { get; set; }

        [Column(TypeName = "date")]
        public DateOnly DateOfCollection { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string CatagoryOfSource { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string CollectingSource { get; set; }

        [Column(TypeName = "date")]
        public DateOnly AnalyzedDate { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string phiAreaName { get; set; }

        [Column(TypeName = "varchar(8)")]
        public string Acceptance { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string Comments { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string PhiId { get; set; }

        [ForeignKey("PHIArea")]
        [Column(TypeName = "varchar(40)")]
        public string PHIAreaId { get; set; }
        public virtual PHIArea PHIArea { get; set; }
        //public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual Report Reports { get; set; }
    }
}