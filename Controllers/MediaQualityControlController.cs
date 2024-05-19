using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.MediaQC;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.IdGeneratorService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MediaQualityControlController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;

        public MediaQualityControlController(ApplicationDBContext context,
                                             UserManager<SystemUser> userManager,
                                             IIdGenerator idGenerator) {
            _context = context;
            _userManager = userManager;
            _idGenerator = idGenerator;
        }

        [HttpGet]
        [Route("GetMediaQualityControlRecords")]
        public async Task<IActionResult> GetMediaQualityControlRecords(String mltId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltId);

                if (mlt == null) {
                    return NotFound();
                }

                var lab = await _context.Labs.FindAsync(mlt.LabID);

                if (lab == null) {
                    return NotFound();
                }

                var mediaQualityControlRecords = await _context.MediaQualityControls
                    .Where(mqcr => mqcr.LabId == lab.LabID)
                    .Select(mqcr => new {
                        mqcr.MediaQualityControlID,
                        mqcr.MediaId,
                        mqcr.DateTime,
                        mqcr.Sterility,
                        mqcr.Stability,
                        mqcr.Sensitivity,
                        mqcr.Remarks,
                        mqcr.LabId
                    })
                    .ToListAsync();

                return Ok(mediaQualityControlRecords);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddMediaQualityControlRecord")]
        public async Task<IActionResult> AddMediaQualityControlRecord([FromBody] MediaQualityRecord newRecord) {
            try {
                var lab = await _context.Labs.FindAsync(newRecord.LabId);

                if (lab == null) {
                    return NotFound();
                }

                var mediaQualityControlRecord = new MediaQualityControl {
                    MediaQualityControlID = _idGenerator.GenerateMediaQualityControlId(),
                    MediaId = newRecord.MediaId,
                    DateTime = newRecord.DateTime,
                    Sterility = newRecord.Sterility,
                    Stability = newRecord.Stability,
                    Sensitivity = newRecord.Sensitivity,
                    Remarks = newRecord.Remarks,
                    MltId = newRecord.MltId,
                    LabId = newRecord.LabId
                };

                await _context.MediaQualityControls.AddAsync(mediaQualityControlRecord);

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateMediaQualityControlRecord/{id}")]
        public async Task<IActionResult> UpdateMediaQualityControlRecord(string id, [FromBody] UpdateMediaQC updatedRecord) {
            try {
                var mediaQualityControlRecord = await _context.MediaQualityControls.FindAsync(id);

                if (mediaQualityControlRecord == null) {
                    return NotFound();
                }

                mediaQualityControlRecord.MediaId = updatedRecord.MediaId;
                mediaQualityControlRecord.DateTime = updatedRecord.DateTime;
                mediaQualityControlRecord.Sterility = updatedRecord.Sterility;
                mediaQualityControlRecord.Stability = updatedRecord.Stability;
                mediaQualityControlRecord.Sensitivity = updatedRecord.Sensitivity;
                mediaQualityControlRecord.Remarks = updatedRecord.Remarks;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteMediaQualityControlRecord/{id}")]
        public async Task<IActionResult> DeleteMediaQualityControlRecord(string id) {
            try {
                var mediaQualityControlRecord = await _context.MediaQualityControls.FindAsync(id);

                if (mediaQualityControlRecord == null) {
                    return NotFound();
                }

                _context.MediaQualityControls.Remove(mediaQualityControlRecord);

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
