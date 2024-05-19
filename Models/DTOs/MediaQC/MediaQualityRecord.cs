﻿namespace Project_v1.Models.DTOs.MediaQC {
    public class MediaQualityRecord {
        public string MediaId { get; set; }
        public DateTime DateTime { get; set; }
        public string Sterility { get; set; }
        public string Stability { get; set; }
        public string Sensitivity { get; set; }
        public string Remarks { get; set; }
        public string MltId { get; set; }
        public string LabId { get; set; }
    }
}
