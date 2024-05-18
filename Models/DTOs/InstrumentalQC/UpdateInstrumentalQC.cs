namespace Project_v1.Models.DTOs.InstrumentalQC {
    public class UpdateInstrumentalQC {
        public DateTime DateTime { get; set; }
        public string Instrument { get; set; }
        public double TemperatureFluctuation { get; set; }
        public double PressureGradient { get; set; }
        public string Timer { get; set; }
        public string Sterility { get; set; }
        public string Stability { get; set; }
        public string Remarks { get; set; }
    }
}
