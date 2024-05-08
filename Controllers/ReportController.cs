using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models.WCReport;
using Project_v1.Services.FirebaseStrorage;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.ReportService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;
        private readonly IStorageService _storageService;

        public ReportController(ApplicationDBContext context,
                                IReportService reportService,
                                IIdGenerator idGenerator,
                                IStorageService storageService) {
            _context = context;
            _reportService = reportService;
            _idGenerator = idGenerator;
            _storageService = storageService;
        }

        [HttpPost]
        [Route("uploadPDF")]
        public async Task<IActionResult> GeneratePdf(String reportRefId) {
            try {

                /*var report = _context.Reports.FirstOrDefault(r => r.ReportRefId == reportRefId);

                if(report == null) {
                    return NotFound("Report not found.");
                }

                var reportData = await _context.Reports.
                    Where(report => report.ReportRefId == reportRefId)
                    .Select(report => new FullReport{
                        MyRefNo = report.MyRefNo,
                        IssuedDate = report.IssuedDate,
                        AppearanceOfSample = report.AppearanceOfSample,
                        EcoliCount = report.EcoliCount,
                        PresumptiveColiformCount = report.PresumptiveColiformCount,
                        Results = report.Remarks,
                        LabName = report.Lab.LabName,
                        LabLocation = report.Lab.LabLocation,
                        LabTelephone = report.Lab.LabTelephone,
                        YourRefNo = report.Sample.YourRefNo,
                        CollectingSource = report.Sample.CollectingSource,
                        DateOfCollection = report.Sample.DateOfCollection,
                        AnalyzedDate = report.Sample.AnalyzedDate,
                        StateOfChlorination = report.Sample.StateOfChlorination
                        })
                    .FirstOrDefaultAsync();
                    
                byte[] pdf = _reportService.GenerateWaterQualityReport(reportData);

                var upload = await _storageService.UploadFile(new MemoryStream(pdf), "WaterQualityReport");*/

                var delete = await _storageService.DeleteFile(reportRefId);

                return Ok("deleted");

            } catch (Exception ex) {
                return StatusCode(500, "An error occurred while generating the PDF." + ex);
            }
        }
    }
}
