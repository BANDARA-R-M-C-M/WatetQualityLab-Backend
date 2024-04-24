using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.Areas;
using Project_v1.Models.Response;
using Project_v1.Models.Users;
using Project_v1.Models.WCReport;
using System.Linq;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WCReportController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;

        public WCReportController(ApplicationDBContext context, UserManager<SystemUser> userManager) {
            _context = context;
            _userManager = userManager;
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
        [Route("newsamples")]
        public async Task<IActionResult> GetNewSamples(String userId) {
            try {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null) {
                    return NotFound($"User with username '{userId}' not found.");
                }

                if (user.LabID == null) {
                    return NotFound($"User with username '{userId}' have a Lab assigned.");
                }
                 
                var mohArea = await _context.MOHAreas.Where(m => m.LabID == user.LabID).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{userId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds.Contains(phi.MOHAreaId)).ToListAsync();

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();

                var samples = await _context.Samples
                    .Where(sample => phiAreaIds.Contains(sample.PHIAreaId))
                    .ToListAsync();

                return Ok(samples);
            } catch (Exception e) {
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
                    SampleId = wcsample.SampleRefId,
                    StateOfChlorination = wcsample.StateOfChlorination,
                    DateOfCollection = DateOnly.ParseExact(wcsample.DateOfCollection, "yyyy-mm-dd", null),
                    CatagoryOfUse = wcsample.CatagoryOfUse,
                    CollectingSource = wcsample.CollectingSource,
                    Phi_Area = wcsample.Phi_Area,
                    PHIAreaId = wcsample.PHIAreaID
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

                var report = new Report {
                    ReportRefId = wcreport.ReportRefId,
                    PresumptiveColiformCount = wcreport.PresumptiveColiformCount,
                    IssuedDate = wcreport.IssuedDate,
                    EcoliCount = wcreport.EcoliCount,
                    AppearanceOfSample = wcreport.AppearanceOfSample,
                    Results = wcreport.Results,
                    Remarks = wcreport.Remarks,
                    SampleId = wcreport.SampleRefId,
                    LabId = wcreport.LabId
                };

                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "WC Report added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
    }
}