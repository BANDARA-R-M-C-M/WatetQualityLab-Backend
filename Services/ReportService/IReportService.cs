using Project_v1.Models.DTOs.GeneralInventoryItems;
using Project_v1.Models.DTOs.SurgicalInventoryItems;
using Project_v1.Models.DTOs.WCReport;

namespace Project_v1.Services.ReportService
{
    public interface IReportService {
        byte[] GenerateWaterQualityReport(FullReport wcreport);
        byte[] GenerateSampleCountReport(List<SampleCount> sampleCount, int year);
        byte[] GenerateInventoryDurationReport(List<GeneralInventoryReport> inventories, int year);
        byte[] GenerateItemIssuingReport(List<ItemIssuingReport> issuedItems, int year, int month);
    }
}
