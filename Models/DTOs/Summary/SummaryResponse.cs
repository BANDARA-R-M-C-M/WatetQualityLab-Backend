using Project_v1.Models.DTOs.SampleCount;

namespace Project_v1.Models.DTOs.Summary {
    public class SummaryResponse {
        public int Year { get; set; }
        public List<MonlthSummary> Months { get; set; }
    }
}
