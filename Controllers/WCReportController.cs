using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.Response;
using Project_v1.Models.Users;
using Project_v1.Models.WCReport;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.ReportService;
using System.Linq;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WCReportController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;

        public WCReportController(ApplicationDBContext context, 
                                  UserManager<SystemUser> userManager,
                                  IReportService reportService,
                                  IIdGenerator idGenerator) {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
            _idGenerator = idGenerator;
        }

        [HttpGet]
        [Route("GetWCSamples")]
        public async Task<IActionResult> GetWCSamples() {
            try {
                var samples = await _context.Samples.ToListAsync();
                return Ok(samples);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetWCReports")]
        public async Task<IActionResult> GetWCReports() {
            try {
                var reports = await _context.Reports.ToListAsync();
                return Ok(reports);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("getAddedSamples")]
        public async Task<IActionResult> GetAddedSamples(String phiId) {
            try {
                var phi = await _userManager.FindByIdAsync(phiId);

                if (phi == null) {
                    return NotFound($"User with username '{phiId}' not found.");
                }

                if (phi.PHIAreaId == null) {
                    return NotFound($"User with username '{phiId}' have a PHI Area assigned.");
                }

                var samples = await _context.Samples
                    .Where(sample => sample.PhiId == phiId)
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.Comments
                    })
                    .ToListAsync();

                return Ok(samples);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("newsamples")]
        public async Task<IActionResult> GetNewSamples(String mltId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltId);

                if (mlt == null) {
                    return NotFound($"User with username '{mltId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{mltId}' have a Lab assigned.");
                }
                 
                var mohArea = await _context.MOHAreas.Where(m => m.LabID == mlt.LabID).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{mltId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds
                    .Contains(phi.MOHAreaId))
                    .ToListAsync();

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();

                var samples = await _context.Samples
                    .Where(sample => phiAreaIds
                    .Contains(sample.PHIAreaId))
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.AnalyzedDate,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.PHIArea.MOHArea.LabID,
                        sample.PHIArea.MOHArea.Lab.LabName,
                        sample.PHIArea.MOHArea.Lab.LabLocation,
                        sample.PHIArea.MOHArea.Lab.LabTelephone,
                        ReportAvailable = _context.Reports.Any(r => r.SampleId == sample.SampleId)
                    })
                    .ToListAsync();

                return Ok(samples);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("getAddedReports")]
        public async Task<IActionResult> GetAddedReports(String mltId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltId);

                if (mlt == null) {
                    return NotFound($"User with username '{mltId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{mltId}' have a Lab assigned.");
                }

                var reports = await _context.Reports
                    .Where(report => report.LabId == mlt.LabID)
                    .Select(report => new {
                        report.Sample.SampleId,
                        report.Sample.StateOfChlorination,
                        report.Sample.DateOfCollection,
                        report.Sample.CatagoryOfSource,
                        report.Sample.CollectingSource,
                        report.Sample.phiAreaName,
                        report.ReportRefId,
                        report.PresumptiveColiformCount,
                        report.IssuedDate,
                        report.EcoliCount,
                        report.AppearanceOfSample,
                        report.Remarks,
                        report.LabId,
                        report.Sample.PHIArea.MOHArea.MOHAreaName
                    })
                    .ToListAsync();

                return Ok(reports);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("newreports")]
        public async Task<IActionResult> GetNewReports(String mohId) {
            try {
                var moh = await _userManager.FindByIdAsync(mohId);

                if (moh == null) {
                    return NotFound($"User with username '{mohId}' not found.");
                }

                if (moh.MOHAreaId == null) {
                    return NotFound($"User with username '{mohId}' have a MOH Area assigned.");
                }

                var mohArea = await _context.MOHAreas.Where(m => m.MOHAreaID == moh.MOHAreaId).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{mohId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds.Contains(phi.MOHAreaId)).ToListAsync();

                if (phiAreas == null) {
                    return NotFound($"No PHIArea found for the MOH Area assigned to user '{mohId}'.");
                }

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();

                var samples = await _context.Samples
                    .Where(sample => phiAreaIds.Contains(sample.PHIAreaId))
                    .Select(sample => new {
                        sample.SampleId,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance
                    })
                    .ToListAsync();

                var sampleIds = samples.Select(s => s.SampleId).ToList();

                var reports = await _context.Reports
                    .Where(report => sampleIds.Contains(report.SampleId))
                    .Select(report => new {
                        report.Sample.SampleId,
                        report.Sample.StateOfChlorination,
                        report.Sample.DateOfCollection,
                        report.Sample.CatagoryOfSource,
                        report.Sample.CollectingSource,
                        report.Sample.phiAreaName,
                        report.ReportRefId,
                        report.PresumptiveColiformCount,    
                        report.IssuedDate,
                        report.EcoliCount,
                        report.AppearanceOfSample,
                        report.Remarks,
                        report.LabId,
                        report.Sample.PHIArea.MOHArea.MOHAreaName
                    })
                    .ToListAsync();

                return Ok(reports);
            } catch (Exception e){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPost]
        [Route("AddWCSample")]
        public async Task<IActionResult> AddWCSample([FromBody] Wc_sample wcsample) {
            try {
                if (wcsample == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var sample = new Sample {
                    SampleId = _idGenerator.GenerateSampleId(),
                    YourRefNo = wcsample.YourRefNo,
                    StateOfChlorination = wcsample.StateOfChlorination,
                    DateOfCollection = wcsample.DateOfCollection,
                    CatagoryOfSource = wcsample.CatagoryOfSource,
                    CollectingSource = wcsample.CollectingSource,
                    Acceptance = "Pending",
                    PhiId = wcsample.phiId,
                    phiAreaName = wcsample.phiAreaName,
                    PHIAreaId = wcsample.PHIAreaID,
                    Comments = ""
                };

                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "WC Sample added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPost]
        [Route("AddWCReport")]
        public async Task<IActionResult> AddWCReport([FromBody] Wc_report wcreport) {
            try {
                if (wcreport == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var sample = await _context.Samples.FindAsync(wcreport.SampleId);

                sample.AnalyzedDate = wcreport.AnalyzedDate;

                var report = new Report {
                    ReportRefId = _idGenerator.GenerateReportId(),
                    MyRefNo = wcreport.MyRefNo,
                    PresumptiveColiformCount = wcreport.PresumptiveColiformCount,
                    IssuedDate = DateTime.Now,
                    EcoliCount = wcreport.EcoliCount,
                    AppearanceOfSample = wcreport.AppearanceOfSample,
                    Remarks = wcreport.Remarks,
                    MltId = wcreport.MltId,
                    SampleId = wcreport.SampleId,
                    LabId = wcreport.LabId
                };

                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "WC Report added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("updateWCSample/{id}")]
        public async Task<IActionResult> UpdateWCSample([FromRoute] String id, [FromBody] Wc_updatedSample updatedSample) {
            try {
                var sample = await _context.Samples.FindAsync(id);

                if (sample == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                if (updatedSample == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid sample data!" });
                }

                sample.YourRefNo = updatedSample.YourRefNo;
                sample.StateOfChlorination = updatedSample.StateOfChlorination;
                sample.DateOfCollection = updatedSample.DateOfCollection;
                sample.CatagoryOfSource = updatedSample.CatagoryOfSource;
                sample.CollectingSource = updatedSample.CollectingSource;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("updateWCReport/{id}")]
        public async Task<IActionResult> UpdateWCReport([FromRoute] String id, [FromBody] Wc_updatedReport updatedReport) {
            try {
                var report = await _context.Reports.FindAsync(id);

                if (report == null) {
                    return NotFound(new Response { Status = "Error", Message = "Report not found!" });
                }

                if (updatedReport == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid report data!" });
                }

                report.MyRefNo = updatedReport.MyRefNo;
                report.AppearanceOfSample = updatedReport.AppearanceOfSample;
                report.PresumptiveColiformCount = updatedReport.PresumptiveColiformCount;
                report.EcoliCount = updatedReport.EcoliCount;
                report.Remarks = updatedReport.Results;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Report updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("updateSampleStatus")]
        public async Task<IActionResult> UpdateSampleStatus([FromBody] SampleStatus sampleStatus) {
            try {
                var sampleToUpdate = await _context.Samples.FindAsync(sampleStatus.SampleId);

                if (sampleToUpdate == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                sampleToUpdate.Acceptance = sampleStatus.Status;
                sampleToUpdate.Comments = sampleStatus.Comment;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample status updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("deleteWCSample/{id}")]
        public async Task<IActionResult> DeleteWCSample([FromRoute] String id) {
            try {
                var sample = await _context.Samples.FindAsync(id);

                if (sample == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                _context.Samples.Remove(sample);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("deleteWCReport/{id}")]
        public async Task<IActionResult> DeleteWCReport([FromRoute] String id) {
            try {
                var report = await _context.Reports.FindAsync(id);

                if (report == null) {
                    return NotFound(new Response { Status = "Error", Message = "Report not found!" });
                }

                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Report deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
            
        [HttpPost]
        [Route("generatePdf")]
        public async Task<IActionResult> GeneratePdf([FromBody] FullReport fullReport) {
            try {
                byte[] pdf = _reportService.GenerateWaterQualityReport(fullReport);

                return File(pdf, "application/pdf", "WaterQualityReport.pdf");
            } catch (Exception ex) {
                // Handle any exceptions
                return StatusCode(500, "An error occurred while generating the PDF." + ex);
            }
        }
    }
}