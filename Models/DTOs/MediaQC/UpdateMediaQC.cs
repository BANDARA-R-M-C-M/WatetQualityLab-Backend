namespace Project_v1.Models.DTOs.MediaQC {
    public class UpdateMediaQC {
        public string Media { get; set; }
        public DateTime DateTime { get; set; }
        public string Sterility { get; set; }
        public string Stability { get; set; }
        public string Sensitivity { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
    }
}
