using Project_v1.Models.DTOs.WCReport;

namespace Project_v1.Services.ReportService
{
    public interface IReportService {
        byte[] GenerateWaterQualityReport(FullReport wcreport);
    }
}
