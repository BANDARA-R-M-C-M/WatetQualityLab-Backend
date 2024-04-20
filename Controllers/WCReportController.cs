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

                /*var labs = await _context.Labs.Where(lab => lab.LabID == user.LabID).ToListAsync();*/

                var phiareas  = await _context.PHIAreas.Where(phi => phi.LabID == user.LabID).ToListAsync();

                var phiAreaIds = phiareas.Select(pa => pa.PHIAreaID).ToList();

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
                    SampleRefId = wcsample.SampleRefId,
                    StateOfChlorination = wcsample.StateOfChlorination,
                    DateOfCollection = DateOnly.ParseExact(wcsample.DateOfCollection, "yyyy-mm-dd", null),
                    CatagoryOfUse = wcsample.CatagoryOfUse,
                    CollectingSource = wcsample.CollectingSource,
                    Phi_Area = wcsample.Phi_Area,
                    PHIAreaId = wcsample.PHIAreaID
                };

                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "WC Report added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
    }
}
