﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.Areas;
using Project_v1.Models.Response;
using Project_v1.Models.Users;
using Project_v1.Services.IdGeneratorService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;

        public AreaController(ApplicationDBContext context,
                              UserManager<SystemUser> userManager,
                              IIdGenerator idGenerator) {
            _context = context;
            _userManager = userManager;
            _idGenerator = idGenerator;
        }

        [HttpGet]
        [Route("GetMOHAreas")]
        public async Task<IActionResult> GetMOHAreas() {
            try {
                var mohareas = await _context.MOHAreas.ToListAsync();

                var mohareasList = mohareas.Select(moharea => new {
                    mohAreaId = moharea.MOHAreaID,
                    mohAreaName = moharea.MOHAreaName,
                    labId = moharea.LabID
                });

                return Ok(mohareasList);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetPHIDetails")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetPHIAreas")]
        public async Task<IActionResult> GetPHIAreas() {
            try {
                var phiaAreas = await _context.PHIAreas.ToListAsync();

                var phiaAreasList = phiaAreas.Select(phiaArea => new {
                    phiAreaId = phiaArea.PHIAreaID,
                    phiAreaName = phiaArea.PHIAreaName,
                    mohAreaId = phiaArea.MOHAreaId
                });

                return Ok(phiaAreasList);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetLabs")]
        public async Task<IActionResult> GetLabs() {
            try {
                var labs = await _context.Labs.ToListAsync();

                var labsList = labs.Select(lab => new {
                    labId = lab.LabID,
                    labName = lab.LabName,
                    labLocation = lab.LabLocation,
                    labTelephone = lab.LabTelephone
                });

                return Ok(labsList);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPost]
        [Route("AddMOHArea")]
        public async Task<IActionResult> AddMOHArea([FromBody] Moh_area moh_area) {
            try {
                if (moh_area == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var moharea = new MOHArea {
                    MOHAreaID = _idGenerator.GenerateMOHAreaId(),
                    MOHAreaName = moh_area.MOHAreaName,
                    LabID = moh_area.LabId
                };

                await _context.MOHAreas.AddAsync(moharea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "MOH Area added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPost]
        [Route("AddPHIArea")]
        public async Task<IActionResult> AddPHIArea([FromBody] Phi_area phia_area) {
            try {
                if (phia_area == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var moh_area = await _context.MOHAreas.FindAsync(phia_area.MOHAreaId);

                if (moh_area == null) {
                    return NotFound(new Response { Status = "Error", Message = "MOH Area not found!" });
                }

                var phiaArea = new PHIArea {
                    PHIAreaID = _idGenerator.GeneratePHIAreaId(),
                    PHIAreaName = phia_area.PHIAreaName,
                    MOHAreaId = phia_area.MOHAreaId,
                };

                await _context.PHIAreas.AddAsync(phiaArea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "PHI Area added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPost]
        [Route("AddLab")]
        public async Task<IActionResult> AddLab([FromBody] lab lab) {
            try {
                if (lab == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var labExists = await _context.Labs.AnyAsync(l => l.LabName == lab.LabName);
                if (labExists) {
                    return BadRequest(new Response { Status = "Error", Message = "Lab already exists!" });
                }

                var newLab = new Lab {
                    LabID = _idGenerator.GenerateLabId(),
                    LabName = lab.LabName,
                    LabLocation = lab.LabLocation,
                    LabTelephone = lab.LabTelephone
                };

                await _context.Labs.AddAsync(newLab);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Lab added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("UpdatePHIArea/{id}")]
        public async Task<IActionResult> UpdatePHIArea([FromRoute] String id ,[FromBody] UpdatedPHIArea phiaArea) {
            try {

                var existingPhiArea = await _context.PHIAreas.FindAsync(id);

                if (existingPhiArea == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "PHI Area not found!" });
                }

                if (phiaArea == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                existingPhiArea.PHIAreaName = phiaArea.phiAreaName;
                existingPhiArea.MOHAreaId = phiaArea.mohAreaId;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "PHI Area updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("UpdateMOHArea/{id}")]
        public async Task<IActionResult> UpdateMOHArea([FromRoute] String id, [FromBody] UpdatedMOHArea mohArea) {
            try {

                var existingMohArea = await _context.MOHAreas.FindAsync(id);

                if (existingMohArea == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "MOH Area not found!" });
                }

                if (mohArea == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                existingMohArea.MOHAreaName = mohArea.mohAreaName;
                existingMohArea.LabID = mohArea.LabId;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "MOH Area updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("UpdateLab/{id}")]
        public async Task<IActionResult> UpdateLab([FromRoute] String id, [FromBody] UpdatedLab lab) {
            try {

                var existingLab = await _context.Labs.FindAsync(id);

                if (existingLab == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Lab not found!" });
                }

                if (lab == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                existingLab.LabName = lab.LabName;
                existingLab.LabLocation = lab.LabLocation;
                existingLab.LabTelephone = lab.LabTelephone;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Lab updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("DeleteMOHArea/{id}")]
        public async Task<IActionResult> DeleteMOHArea([FromRoute]string id) {
            try {
                var moharea = await _context.MOHAreas.FindAsync(id);
                if (moharea == null) {
                    return NotFound(new Response { Status = "Error", Message = "MOH Area not found!" });
                }

                _context.MOHAreas.Remove(moharea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "MOH Area deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("DeletePHIArea/{id}")]
        public async Task<IActionResult> DeletePHIArea([FromRoute]string id) {
            try {
                var phiaArea = await _context.PHIAreas.FindAsync(id);
                if (phiaArea == null) {
                    return NotFound(new Response { Status = "Error", Message = "PHI Area not found!" });
                }

                _context.PHIAreas.Remove(phiaArea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "PHI Area deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("DeleteLab/{id}")]
        public async Task<IActionResult> DeleteLab([FromRoute]string id) {
            try {
                var lab = await _context.Labs.FindAsync(id);
                if (lab == null) {
                    return NotFound(new Response { Status = "Error", Message = "Lab not found!" });
                }

                _context.Labs.Remove(lab);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Lab deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
    }
}
