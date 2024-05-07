using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_v1.Models.WCReport;
using Project_v1.Services.FirebaseStrorage;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.ReportService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase {

        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;
        private readonly IStorageService _storageService;

        public ReportController(IReportService reportService,
                                IIdGenerator idGenerator,
                                IStorageService storageService) {
            _reportService = reportService;
            _idGenerator = idGenerator;
            _storageService = storageService;
        }

        [HttpPost]
        [Route("uploadPDF")]
        public async Task<IActionResult> GeneratePdf([FromBody] FullReport fullReport) {
            try {
                byte[] pdf = _reportService.GenerateWaterQualityReport(fullReport);

                var upload = await _storageService.UploadFile(new MemoryStream(pdf), "WaterQualityReport");

                return Ok(upload);

            } catch (Exception ex) {
                return StatusCode(500, "An error occurred while generating the PDF." + ex);
            }
        }
    }
}
