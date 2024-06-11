using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Areas;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.Logging;
using System.Security.Claims;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;
        private readonly UserActionsLogger _actionsLogger;

        public AreaController(ApplicationDBContext context,
                              UserManager<SystemUser> userManager,
                              IIdGenerator idGenerator,
                              IFilter filter,
                              UserActionsLogger logger) {
            _context = context;
            _userManager = userManager;
            _idGenerator = idGenerator;
            _filter = filter;
            _actionsLogger = logger;
        }

        [HttpGet]
        [Route("GetMOHAreas")]
        [Authorize]
        public async Task<IActionResult> GetMOHAreas([FromQuery] QueryObject query) {
            try {
                var mohareasList = _context.MOHAreas
                    .Select(moharea => new {
                        mohAreaId = moharea.MOHAreaID,
                        mohAreaName = moharea.MOHAreaName,
                        labId = moharea.LabID,
                        labName = moharea.Lab.LabName
                    });

                var filteredResult = await _filter.Filtering(mohareasList, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetPHIDetails")]
        [Authorize]
        public async Task<IActionResult> GetPHIDetails(String phiId) {
            try {
                var phi = await _userManager.FindByIdAsync(phiId);

                if (phi == null) {
                    return NotFound(new Response { Status = "Error", Message = "PHI not found!" });
                }

                var phiArea = await _context.PHIAreas.Where(p => p.PHIAreaID == phi.PHIAreaId).FirstOrDefaultAsync();

                if (phiArea == null) {
                    return NotFound(new Response { Status = "Error", Message = "PHI Area not found!" });
                }

                return Ok(new {
                    phiAreaId = phi.PHIAreaId,
                    phiAreaName = phiArea.PHIAreaName,
                });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetPHIAreas")]
        [Authorize]
        public async Task<IActionResult> GetPHIAreas([FromQuery] QueryObject query) {
            try {
                var phiaAreasList = _context.PHIAreas
                    .Select(phiaArea => new {
                        phiAreaId = phiaArea.PHIAreaID,
                        phiAreaName = phiaArea.PHIAreaName,
                        mohAreaId = phiaArea.MOHAreaId,
                        mohAreaName = phiaArea.MOHArea.MOHAreaName
                    });

                var filteredResult = await _filter.Filtering(phiaAreasList, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetLabs")]
        [Authorize]
        public async Task<IActionResult> GetLabs([FromQuery] QueryObject query) {
            try {
                var labsList = _context.Labs
                    .Select(lab => new {
                        labId = lab.LabID,
                        labName = lab.LabName,
                        labLocation = lab.LabLocation,
                        labTelephone = lab.LabTelephone
                    });

                var filteredResult = await _filter.Filtering(labsList, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddMOHArea")]
        [Authorize]
        public async Task<IActionResult> AddMOHArea([FromBody] Moh_area moh_area) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if(await _context.MOHAreas.AnyAsync(m => m.MOHAreaName == moh_area.MOHAreaName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "MOH Area already exists!" });
                }

                var mohAreaId = _idGenerator.GenerateMOHAreaId();

                var moharea = new MOHArea {
                    MOHAreaID = mohAreaId,
                    MOHAreaName = moh_area.MOHAreaName,
                    LabID = moh_area.LabId
                };

                await _context.MOHAreas.AddAsync(moharea);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Add MOH Area: {mohAreaId} Assigned to: {moh_area.LabId} Added by: {userId} || MOH Area Name: {moh_area.MOHAreaName}");

                return Ok(new Response { Status = "Success", Message = "MOH Area added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddPHIArea")]
        [Authorize]
        public async Task<IActionResult> AddPHIArea([FromBody] Phi_area phia_area) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (await _context.PHIAreas.AnyAsync(p => p.PHIAreaName == phia_area.PHIAreaName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "PHI Area already exists!" });
                }

                var moh_area = await _context.MOHAreas.FindAsync(phia_area.MOHAreaId);

                if (moh_area == null) {
                    return NotFound(new {  Message = "MOH Area not found!" });
                }

                var phiAreaId = _idGenerator.GeneratePHIAreaId();

                var phiaArea = new PHIArea {
                    PHIAreaID = phiAreaId,
                    PHIAreaName = phia_area.PHIAreaName,
                    MOHAreaId = phia_area.MOHAreaId,
                };

                await _context.PHIAreas.AddAsync(phiaArea);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Add PHI Area: {phiAreaId} Assigned to: {phia_area.MOHAreaId} Added by: {userId} || PHI Area Name: {phia_area.PHIAreaName}");

                return Ok(new Response { Status = "Success", Message = "PHI Area added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddLab")]
        [Authorize]
        public async Task<IActionResult> AddLab([FromBody] lab lab) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if(await _context.Labs.AnyAsync(l => l.LabName == lab.LabName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Lab already exists!" });
                }

                var labId = _idGenerator.GenerateLabId();

                var newLab = new Lab {
                    LabID = labId,
                    LabName = lab.LabName,
                    LabLocation = lab.LabLocation,
                    LabTelephone = lab.LabTelephone
                };

                await _context.Labs.AddAsync(newLab);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Add Lab: {labId} Added by: {userId} || Name: {lab.LabName}, Location: {lab.LabLocation}, Telephone: {lab.LabTelephone}");

                return Ok(new Response { Status = "Success", Message = "Lab added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdatePHIArea/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePHIArea([FromRoute] String id, [FromBody] UpdatedPHIArea phiaArea) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var existingPhiArea = await _context.PHIAreas.FindAsync(id);

                if (existingPhiArea == null) {
                    return NotFound();
                }

                if (existingPhiArea.PHIAreaName != phiaArea.phiAreaName) {
                    if (await _context.PHIAreas.AnyAsync(c => c.PHIAreaName == phiaArea.phiAreaName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "PHI Area already exists!" });
                    }
                }

                existingPhiArea.PHIAreaName = phiaArea.phiAreaName;
                existingPhiArea.MOHAreaId = phiaArea.mohAreaId;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Update PHI Area: {id} Updated by: {userId} to Name: {phiaArea.phiAreaName} Asssigned to: {phiaArea.mohAreaId}");

                return Ok(new Response { Status = "Success", Message = "PHI Area updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateMOHArea/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMOHArea([FromRoute] String id, [FromBody] UpdatedMOHArea mohArea) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var existingMohArea = await _context.MOHAreas.FindAsync(id);

                if (existingMohArea == null) {
                    return NotFound();
                }

                if (existingMohArea.MOHAreaName != mohArea.mohAreaName) {
                    if (await _context.MOHAreas.AnyAsync(c => c.MOHAreaName == mohArea.mohAreaName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "PHI Area already exists!" });
                    }
                }

                existingMohArea.MOHAreaName = mohArea.mohAreaName;
                existingMohArea.LabID = mohArea.LabId;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Update MOH Area: {id} Updated by: {userId} to Name: {mohArea.mohAreaName} Asssigned to: {mohArea.LabId}");

                return Ok(new Response { Status = "Success", Message = "MOH Area updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateLab/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLab([FromRoute] String id, [FromBody] UpdatedLab lab) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var existingLab = await _context.Labs.FindAsync(id);

                if (existingLab == null) {
                    return NotFound();
                }

                if (existingLab.LabName != lab.LabName) {
                    if (await _context.Labs.AnyAsync(c => c.LabName == lab.LabName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Lab already exists!" });
                    }
                }

                existingLab.LabName = lab.LabName;
                existingLab.LabLocation = lab.LabLocation;
                existingLab.LabTelephone = lab.LabTelephone;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Update Lab: {id} Updated by: {userId} to Name: {lab.LabName} Location: {lab.LabLocation} Telephone: {lab.LabTelephone}");

                return Ok(new Response { Status = "Success", Message = "Lab updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteMOHArea/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMOHArea([FromRoute] string id) {
            try {
                var moharea = await _context.MOHAreas.FindAsync(id);
                if (moharea == null) {
                    return NotFound(new Response { Status = "Error", Message = "MOH Area not found!" });
                }

                _context.MOHAreas.Remove(moharea);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Delete MOH Area: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "MOH Area deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePHIArea/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePHIArea([FromRoute] string id) {
            try {
                var phiaArea = await _context.PHIAreas.FindAsync(id);
                if (phiaArea == null) {
                    return NotFound(new Response { Status = "Error", Message = "PHI Area not found!" });
                }

                _context.PHIAreas.Remove(phiaArea);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Delete PHI Area: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "PHI Area deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteLab/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLab([FromRoute] string id) {
            try {
                var lab = await _context.Labs.FindAsync(id);
                if (lab == null) {
                    return NotFound(new Response { Status = "Error", Message = "Lab not found!" });
                }

                _context.Labs.Remove(lab);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Delete Lab: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Lab deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
