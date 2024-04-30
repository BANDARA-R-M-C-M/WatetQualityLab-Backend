using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.Areas;
using Project_v1.Models.Response;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase {

        private readonly ApplicationDBContext _context;

        public AreaController(ApplicationDBContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("GetMOHAreas")]
        public async Task<IActionResult> GetMOHAreas() {
            try {
                var mohareas = await _context.MOHAreas.Include(MOHArea => MOHArea.PHIAreas).ToListAsync();
                return Ok(mohareas);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetPHIAreas")]
        public async Task<IActionResult> GetPHIAreas() {
            try {
                var phiareas = await _context.PHIAreas.ToListAsync();

                var phiAreas = phiareas
                .Select(phiarea => new {
                    phiarea.PHIAreaID,
                    phiarea.PHIAreaName
                })
                .ToList();

                return Ok(phiAreas);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("GetLabs")]
        public async Task<IActionResult> GetLabs() {
            try {
                var labs = await _context.Labs.ToListAsync();

                return Ok(labs);
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
                    MOHAreaID = moh_area.MOHAreaID,
                    MOHAreaName = moh_area.MOHAreaName,
                    LabID = moh_area.LabID
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
                    PHIAreaID = phia_area.PHIAreaID,
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

                var labExists = await _context.Labs.AnyAsync(l => l.LabID == lab.LabID);
                if (labExists) {
                    return BadRequest(new Response { Status = "Error", Message = "Lab already exists!" });
                }

                var newLab = new Lab {
                    LabID = lab.LabID,
                    Lab_name = lab.Lab_name,
                    Lab_location = lab.Lab_location,
                    Lab_telephone = lab.Lab_telephone
                };

                await _context.Labs.AddAsync(newLab);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Lab added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("UpdateMOHArea")]
        public async Task<IActionResult> UpdateMOHArea([FromBody] MOHArea moharea) {
            try {
                if (moharea == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                _context.MOHAreas.Update(moharea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "MOH Area updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("UpdatePHIArea/{id}")]
        public async Task<IActionResult> UpdatePHIArea([FromRoute] String id ,[FromBody] PHIArea phiaArea) {
            try {

                var phiarea = await _context.PHIAreas.FindAsync(id);

                if (phiarea == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "PHI Area not found!" });
                }

                if (phiaArea == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                _context.PHIAreas.Update(phiaArea);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "PHI Area updated successfully!" });
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
    }
}
