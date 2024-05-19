using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class MediaQualityControl {
        [Key]
        public string MediaQualityControlID { get; set; }
        public string MediaId {  get; set; }
        public DateTime DateTime { get; set; }
        public string Sterility { get; set; }
        public string Stability { get; set; }
        public string Sensitivity { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
    }
}
