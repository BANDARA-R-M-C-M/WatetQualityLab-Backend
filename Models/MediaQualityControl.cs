using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class MediaQualityControl {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string MediaQualityControlID { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string MediaId {  get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string Sterility { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string Stability { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string Sensitivity { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Remarks { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string MltId { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
    }
}
