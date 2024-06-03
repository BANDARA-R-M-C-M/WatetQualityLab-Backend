using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Assigns;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.Logging;
using System.Security.Claims;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserassignController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly UserActionsLogger _actionsLogger;

        public UserassignController(ApplicationDBContext context,
                                    UserManager<SystemUser> userManager,
                                    UserActionsLogger actionsLogger) {
            _context = context;
            _userManager = userManager;
            _actionsLogger = actionsLogger;
        }

        [HttpPost]
        [Route("AssignMLTtoLabs")]
        public async Task<IActionResult> AssignMLTtoLabs([FromBody] mltLab mltlab) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltlab.mltId);

                if (mlt == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                if (mltlab.labId == null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Lab not found" });
                }

                mlt.LabID = mltlab.labId;
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"MLT {mlt.Id} assigned to Lab {mlt.LabID} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Lab assigned successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AssignPHItoPHIAreas")]
        public async Task<IActionResult> AssignPHItoPHIArea([FromBody] phiPhiarea phi_phiarea) {
            try {
                var phi = await _userManager.FindByIdAsync(phi_phiarea.phiId);

                if (phi == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                if (phi_phiarea.phiAreaId == null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "PHI Area not found" });
                }

                phi.PHIAreaId = phi_phiarea.phiAreaId;
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"PHI {phi.Id} assigned to PHI Area {phi.PHIAreaId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "PHI Area assigned successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AssignMOHSupervisortoMOHAreas")]
        public async Task<IActionResult> AssignMOHSupervisortoMOHArea([FromBody] mohMoharea moh_moharea) {
            try {
                var moh = await _userManager.FindByIdAsync(moh_moharea.mohSupervisorId);

                if (moh == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                if (moh_moharea.mohAreaId == null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "MOH Area not found" });
                }

                moh.MOHAreaId = moh_moharea.mohAreaId;
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"MOH Supervisor {moh.Id} assigned to MOH Area {moh.MOHAreaId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "MOH Area assigned successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
