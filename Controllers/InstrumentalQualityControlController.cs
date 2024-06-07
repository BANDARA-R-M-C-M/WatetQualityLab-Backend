using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.InstrumentalQC;
using Project_v1.Models.DTOs.Response;
using Project_v1.Models.DTOs.SurgicalInventoryItems;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.Logging;
using System.Security.Claims;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentalQualityControlController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;
        private readonly UserActionsLogger _actionsLogger;

        public InstrumentalQualityControlController(ApplicationDBContext context,
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
        [Route("GetInstrumentalQualityControlRecords")]
        [Authorize]
        public async Task<IActionResult> GetInstrumentalQualityControlRecords([FromQuery] QueryObject query) {
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
                    return NotFound();
                }

                var instrumentalQualityControlRecords = _context.InstrumentalQualityControls
                    .Where(iqcr => iqcr.LabId == lab.LabID)
                    .Select(iqcr => new {
                        iqcr.InstrumentalQualityControlID,
                        iqcr.DateTime,
                        iqcr.InstrumentId,
                        iqcr.TemperatureFluctuation,
                        iqcr.PressureGradient,
                        iqcr.Timer,
                        iqcr.Sterility,
                        iqcr.Stability,
                        iqcr.Remarks,
                        iqcr.LabId
                    });

                var filteredResult = await _filter.Filtering(instrumentalQualityControlRecords, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddInstrumentalQualityControlRecord")]
        [Authorize]
        public async Task<IActionResult> AddInstrumentalQualityControlRecord([FromBody] InstrumentalQualityRecord newRecord) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (newRecord.DateTime > DateTime.Now) {
                    return BadRequest(new { Message = "Record's Date Time cannot be in the future!" });
                }

                var lab = await _context.Labs.FindAsync(newRecord.LabId);

                if (lab == null) {
                    return NotFound();
                }

                var instrumentQualityId = _idGenerator.GenerateInstrumentalQualityControlId();

                var newQCRecord = new InstrumentalQualityControl {
                    InstrumentalQualityControlID = instrumentQualityId,
                    DateTime = newRecord.DateTime,
                    InstrumentId = newRecord.InstrumentId,
                    TemperatureFluctuation = (double)newRecord.TemperatureFluctuation,
                    PressureGradient = (double)newRecord.PressureGradient,
                    Timer = newRecord.Timer,
                    Sterility = newRecord.Sterility,
                    Stability = newRecord.Stability,
                    Remarks = newRecord.Remarks,
                    MltId = newRecord.MltId,
                    LabId = newRecord.LabId
                };

                await _context.InstrumentalQualityControls.AddAsync(newQCRecord);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Added Instrumental Quality Record: {instrumentQualityId} Added by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateInstrumentalQualityControlRecord/{id}")]
        public async Task<IActionResult> UpdateInstrumentalQualityControlRecord([FromRoute] String id, [FromBody] UpdateInstrumentalQC instrumentalQC) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (instrumentalQC.DateTime > DateTime.Now) {
                    return BadRequest(new { Message = "Record's Date Time cannot be in the future!" });
                }

                var qcRecord = await _context.InstrumentalQualityControls.FindAsync(id);

                if (qcRecord == null) {
                    return NotFound();
                }

                qcRecord.DateTime = instrumentalQC.DateTime;
                qcRecord.InstrumentId = instrumentalQC.InstrumentId;
                qcRecord.TemperatureFluctuation = (double)instrumentalQC.TemperatureFluctuation;
                qcRecord.PressureGradient = (double)instrumentalQC.PressureGradient;
                qcRecord.Timer = instrumentalQC.Timer;
                qcRecord.Sterility = instrumentalQC.Sterility;
                qcRecord.Stability = instrumentalQC.Stability;
                qcRecord.Remarks = instrumentalQC.Remarks;
                qcRecord.MltId = instrumentalQC.MltId;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Updated Instrumental Quality Record: {id} Updated by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteInstrumentalQualityControlRecord/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInstrumentalQualityControlRecord([FromRoute] String id) {
            try {
                var qcRecord = await _context.InstrumentalQualityControls.FindAsync(id);

                if (qcRecord == null) {
                    return NotFound();
                }

                _context.InstrumentalQualityControls.Remove(qcRecord);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _actionsLogger.LogInformation($"Deleted Instrumental Quality Record: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
