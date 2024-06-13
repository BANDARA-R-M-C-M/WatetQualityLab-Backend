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
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.Response;
using Project_v1.Models.DTOs.WCReport;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.Logging;
using Project_v1.Services.ReportService;
using System.Composition;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WCReportController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;
        private readonly UserActionsLogger _actionsLogger;

        public WCReportController(ApplicationDBContext context,
                                  UserManager<SystemUser> userManager,
                                  IReportService reportService,
                                  IIdGenerator idGenerator,
                                  IFilter filter,
                                  UserActionsLogger actionsLogger) {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
            _idGenerator = idGenerator;
            _filter = filter;
            _actionsLogger = actionsLogger;
        }

        [HttpGet]
        [Route("GetWCReports")]
        [Authorize]
        public async Task<IActionResult> GetWCReports() {
            try {
                var reports = await _context.Reports.ToListAsync();
                return Ok(reports);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getAddedReports")]
        [Authorize]
        public async Task<IActionResult> GetAddedReports([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "User ID is required!" });
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"No Laboratory assigned.");
                }

                var reports = _context.Reports
                    .Where(report => report.LabId == mlt.LabID)
                    .Select(report => new {
                        report.Sample.SampleId,
                        report.Sample.StateOfChlorination,
                        report.Sample.DateOfCollection,
                        report.Sample.CatagoryOfSource,
                        report.Sample.CollectingSource,
                        report.Sample.phiAreaName,
                        report.ReportRefId,
                        report.MyRefNo,
                        report.PresumptiveColiformCount,
                        report.IssuedDate,
                        report.EcoliCount,
                        report.AppearanceOfSample,
                        report.Remarks,
                        report.Contaminated,
                        report.LabId,
                        report.Sample.PHIArea.MOHArea.MOHAreaName
                    });

                var filteredResult = await _filter.Filtering(reports, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("newreports")]
        [Authorize]
        public async Task<IActionResult> GetNewReports([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "User ID is required!" });
                }

                var moh = await _userManager.FindByIdAsync(query.UserId);

                if (moh == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (moh.MOHAreaId == null) {
                    return NotFound($"No MOH Area assigned.");
                }

                var mohArea = await _context.MOHAreas.Where(m => m.MOHAreaID == moh.MOHAreaId).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{query.UserId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds.Contains(phi.MOHAreaId)).ToListAsync();

                if (phiAreas == null) {
                    return NotFound($"No PHIArea found for the MOH Area assigned to user '{query.UserId}'.");
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

                var reports = _context.Reports
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
                    });

                var filteredResult = await _filter.Filtering(reports, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetContaminationDetails")]
        public async Task<IActionResult> GetContaminationDetails([FromQuery] QueryObject query, [FromQuery] int? Month = null, [FromQuery] int? Year = null) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var moh = await _userManager.FindByIdAsync(query.UserId);

                if (moh == null) {
                    return NotFound();
                }

                if (moh.MOHAreaId == null) {
                    return NotFound("No MOH Area assigned!");
                }

                var phiAreaList = _context.PHIAreas
                .Where(phi => phi.MOHAreaId == moh.MOHAreaId)
                .Select(phi => new {
                    phi.PHIAreaID,
                    phi.PHIAreaName,
                    phi.MOHArea.MOHAreaName,
                    ContaminationDetails = _context.Reports
                        .Where(report => report.Sample.PHIAreaId == phi.PHIAreaID &&
                                (Month == null || report.IssuedDate.Month == Month) &&
                                (Year == null || report.IssuedDate.Year == Year))
                        .Select(report => new {
                            report.Contaminated,
                            report.IssuedDate
                        }).ToList(),
                    ReportAvailableForMonth = _context.Reports
                        .Any(report => report.Sample.PHIAreaId == phi.PHIAreaID &&
                                        (Month == null || report.IssuedDate.Month == Month) &&
                                        (Year == null || report.IssuedDate.Year == Year))
                });

                var filteredResult = await _filter.Filtering(phiAreaList, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetMOHAreaDetails")]
        [Authorize]
        public async Task<IActionResult> GetReportDetails([FromQuery] QueryObject query, [FromQuery] int? Month = null, [FromQuery] int? Year = null) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var moh = await _userManager.FindByIdAsync(query.UserId);

                if (moh == null) {
                    return NotFound();
                }

                if (moh.MOHAreaId == null) {
                    return NotFound("No MOH Area assigned!");
                }

                var phiAreaList = await _context.PHIAreas
                .Where(phi => phi.MOHAreaId == moh.MOHAreaId)
                .Select(phi => new {
                    phi.PHIAreaID,
                    phi.PHIAreaName,
                    phi.MOHAreaId,
                    phi.MOHArea.MOHAreaName,
                    Contamination = _context.Reports
                        .Where(report => report.Sample.PHIAreaId == phi.PHIAreaID &&
                                            (Month == null || report.IssuedDate.Month == Month) &&
                                            (Year == null || report.IssuedDate.Year == Year))
                        .Select(report => new {
                            report.Contaminated
                        }).ToList()
                }).ToListAsync();

                return Ok(phiAreaList);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetReportPDF")]
        [Authorize]
        public async Task<IActionResult> GetReportPDF(String reportId) {
            try {
                var report = await _context.Reports.FindAsync(reportId);

                if (report == null) {
                    return NotFound();
                }

                var sample = await _context.Samples.FindAsync(report.SampleId);

                if (sample == null) {
                    return NotFound();
                }

                var lab = await _context.Labs.FindAsync(report.LabId);

                if (lab == null) {
                    return NotFound();
                }

                var newReportData = new FullReport {
                    MyRefNo = report.MyRefNo,
                    IssuedDate = report.IssuedDate,
                    AppearanceOfSample = report.AppearanceOfSample,
                    EcoliCount = report.EcoliCount,
                    PresumptiveColiformCount = report.PresumptiveColiformCount,
                    Results = report.Remarks,
                    YourRefNo = sample.YourRefNo,
                    CollectingSource = sample.CollectingSource,
                    DateOfCollection = sample.DateOfCollection,
                    AnalyzedDate = sample.AnalyzedDate,
                    StateOfChlorination = sample.StateOfChlorination,
                    LabName = lab.LabName,
                    LabLocation = lab.LabLocation,
                    LabTelephone = lab.LabTelephone
                };

                byte[] wcreport = _reportService.GenerateWaterQualityReport(newReportData);

                return File(wcreport, "application/pdf", reportId);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetComments")]
        [Authorize]
        public async Task<IActionResult> GetComments() {
            try {
                var comments = await _context.Comments.ToListAsync();
                return Ok(comments);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddWCReport")]
        [Authorize]
        public async Task<IActionResult> AddWCReport([FromBody] Wc_report wcreport) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var sample = await _context.Samples.FindAsync(wcreport.SampleId);

                if (sample == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                if (await _context.Reports.AnyAsync(r => r.MyRefNo == wcreport.MyRefNo)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "YourRefNo already exists!" });
                }

                if (wcreport.AnalyzedDate < sample.DateOfCollection) {
                    return BadRequest(new { Message = "Analyzed date cannot earlier than date of collection!" });
                }

                if (wcreport.AnalyzedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new { Message = "Analyzed Date cannot be in the future!" });
                }

                var lab = await _context.Labs.FindAsync(wcreport.LabId);

                if (lab == null) {
                    return NotFound(new Response { Status = "Error", Message = "Lab not found!" });
                }

                var reportId = _idGenerator.GenerateReportId();
                var issuedDate = DateOnly.FromDateTime(DateTime.Now);

                var report = new Report {
                    ReportRefId = reportId,
                    MyRefNo = wcreport.MyRefNo,
                    PresumptiveColiformCount = wcreport.PresumptiveColiformCount,
                    IssuedDate = issuedDate,
                    EcoliCount = wcreport.EcoliCount,
                    AppearanceOfSample = wcreport.AppearanceOfSample,
                    Remarks = wcreport.Remarks,
                    Contaminated = wcreport.Contaminated,
                    MltId = wcreport.MltId,
                    SampleId = wcreport.SampleId,
                    LabId = wcreport.LabId
                };

                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Added Water Quality Report: {reportId} Added by: {userId}");

                return Ok(new Response { Status = "Success", Message = "WC Report added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddComment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] Comment comment) {
            try {
                if (comment == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid comment data!" });
                }

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Added Comment: {comment.CommentId} Added by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Comment added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("updateWCReport/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWCReport([FromRoute] String id, [FromBody] Wc_updatedReport updatedReport) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var report = await _context.Reports.FindAsync(id);

                if (report == null) {
                    return NotFound(new Response { Status = "Error", Message = "Report not found!" });
                }

                if (report.MyRefNo != updatedReport.MyRefNo) {
                    if (await _context.Reports.AnyAsync(s => s.MyRefNo == updatedReport.MyRefNo)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "MyRefNo already exists!" });
                    }
                }

                report.MyRefNo = updatedReport.MyRefNo;
                report.AppearanceOfSample = updatedReport.AppearanceOfSample;
                report.PresumptiveColiformCount = updatedReport.PresumptiveColiformCount;
                report.EcoliCount = updatedReport.EcoliCount;
                report.Remarks = updatedReport.Remarks;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Updated Water Quality Report: {id} Updated by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Report updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("updateComment/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromRoute] String id, [FromBody] Comment comment) {
            try {
                var existingComment = await _context.Comments.FindAsync(id);

                if (existingComment == null) {
                    return NotFound(new Response { Status = "Error", Message = "Comment not found!" });
                }

                if (comment == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid comment data!" });
                }

                existingComment.Feedback = comment.Feedback;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Updated Comment: {id} Updated by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Comment updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteWCReport/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWCReport([FromRoute] String id) {
            try {
                var report = await _context.Reports.FindAsync(id);

                if (report == null) {
                    return NotFound(new Response { Status = "Error", Message = "Report not found!" });
                }

                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Deleted Water Quality Report: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Report deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteComment/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] String id) {
            try {
                var comment = await _context.Comments.FindAsync(id);

                if (comment == null) {
                    return NotFound(new Response { Status = "Error", Message = "Comment not found!" });
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Deleted Comment: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Comment deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}