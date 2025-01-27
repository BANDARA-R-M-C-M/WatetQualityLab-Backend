﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.MediaQC;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.Logging;
using System.Security.Claims;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MediaQualityControlController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;
        private readonly UserActionsLogger _actionsLogger;

        public MediaQualityControlController(ApplicationDBContext context,
                                             UserManager<SystemUser> userManager,
                                             IIdGenerator idGenerator,
                                             IFilter filter,
                                             UserActionsLogger actionsLogger) {
            _context = context;
            _userManager = userManager;
            _idGenerator = idGenerator;
            _filter = filter;
            _actionsLogger = actionsLogger;
        }

        [HttpGet]
        [Route("GetMediaQualityControlRecords")]
        [Authorize]
        public async Task<IActionResult> GetMediaQualityControlRecords([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound();
                }

                var lab = await _context.Labs.FindAsync(mlt.LabID);

                if (lab == null) {
                    return NotFound("No Laboratory Assigned!");
                }

                var mediaQualityControlRecords = _context.MediaQualityControls
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
                    });

                var filteredResult = await _filter.Filtering(mediaQualityControlRecords, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddMediaQualityControlRecord")]
        [Authorize]
        public async Task<IActionResult> AddMediaQualityControlRecord([FromBody] MediaQualityRecord newRecord) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (newRecord.DateTime > DateTime.Now) {
                    return BadRequest(new { Message = "Record's Date Time cannot be in the future!" });
                }

                var lab = await _context.Labs.FindAsync(newRecord.LabId);

                if (lab == null) {
                    return NotFound();
                }

                var mediaQualityRecord = _idGenerator.GenerateMediaQualityControlId();

                var mediaQualityControlRecord = new MediaQualityControl {
                    MediaQualityControlID = mediaQualityRecord,
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

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Added Media Quality Control Record: {mediaQualityRecord} Added by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateMediaQualityControlRecord/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMediaQualityControlRecord(string id, [FromBody] UpdateMediaQC updatedRecord) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (updatedRecord.DateTime > DateTime.Now) {
                    return BadRequest(new { Message = "Record's Date Time cannot be in the future!" });
                }

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

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Updated Media Quality Control Record: {id} Updated by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteMediaQualityControlRecord/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMediaQualityControlRecord(string id) {
            try {
                var mediaQualityControlRecord = await _context.MediaQualityControls.FindAsync(id);

                if (mediaQualityControlRecord == null) {
                    return NotFound();
                }

                _context.MediaQualityControls.Remove(mediaQualityControlRecord);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Deleted Media Quality Control Record: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Media Quality Control Record deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
