namespace Project_v1.Models.DTOs.SampleCount {
    public class SampleCountResponse {
        public int Year { get; set; }
        public List<MonthSampleCount> Months { get; set; }
    }
}
