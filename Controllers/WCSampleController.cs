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
using Project_v1.Services.ReportService;

namespace Project_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WCSampleController : ControllerBase {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;

        public WCSampleController(ApplicationDBContext context,
                                  UserManager<SystemUser> userManager,
                                  IReportService reportService,
                                  IIdGenerator idGenerator,
                                  IFilter filter) {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
            _idGenerator = idGenerator;
            _filter = filter;
        }

        [HttpGet]
        [Route("GetWCSamples")]
        public async Task<IActionResult> GetWCSamples() {
            try {
                var samples = await _context.Samples.ToListAsync();
                return Ok(samples);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("newsamples")]
        public async Task<IActionResult> GetNewSamples([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{query.UserId}' have a Lab assigned.");
                }

                var mohArea = await _context.MOHAreas.Where(m => m.LabID == mlt.LabID).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{query.UserId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds
                    .Contains(phi.MOHAreaId))
                    .ToListAsync();

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();

                var samples = _context.Samples
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
                    });

                var searchResults = _filter.Search(samples, query.YourRefNo, "YourRefNo");
                var sortedResult = _filter.Sort(searchResults, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);

                return Ok(result);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
