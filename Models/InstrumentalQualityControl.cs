using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class InstrumentalQualityControl {
        [Key]
        public string InstrumentalQualityControlID { get; set; }
        public DateTime DateTime { get; set; }
        public string InstrumentId { get; set; }
        public double TemperatureFluctuation { get; set; }
        public double PressureGradient { get; set; }
        public string Timer { get; set; }
        public string Sterility { get; set; }
        public string Stability { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
    }
}
