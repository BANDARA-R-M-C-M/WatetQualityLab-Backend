using Project_v1.Models.WCReport;

namespace Project_v1.Services.ReportService {
    public interface IReportService {
        public byte[] GenerateWaterQualityReport(FullReport wcreport);
    }
}
