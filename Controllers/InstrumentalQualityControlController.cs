using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.InstrumentalQC;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;

namespace Project_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentalQualityControlController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;

        public InstrumentalQualityControlController(ApplicationDBContext context,
                                                    UserManager<SystemUser> userManager,
                                                    IIdGenerator idGenerator,
                                                    IFilter filter) {
            _context = context;
            _userManager = userManager;
            _idGenerator = idGenerator;
            _filter = filter;
        }

        [HttpGet]
        [Route("GetInstrumentalQualityControlRecords")]
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

                var searchResult = _filter.Search(instrumentalQualityControlRecords, query.InstrumentId, "InstrumentId");
                var sortedResult = _filter.Sort(searchResult, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);

                return Ok(result);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddInstrumentalQualityControlRecord")]
        public async Task<IActionResult> AddInstrumentalQualityControlRecord([FromBody] InstrumentalQualityRecord newRecord) {
            try {
                var lab = await _context.Labs.FindAsync(newRecord.LabId);

                if (lab == null) {
                    return NotFound();
                }

                var newQCRecord = new InstrumentalQualityControl {
                    InstrumentalQualityControlID = _idGenerator.GenerateInstrumentalQualityControlId(),
                    DateTime = newRecord.DateTime,
                    InstrumentId = newRecord.InstrumentId,
                    TemperatureFluctuation = newRecord.TemperatureFluctuation,
                    PressureGradient = newRecord.PressureGradient,
                    Timer = newRecord.Timer,
                    Sterility = newRecord.Sterility,
                    Stability = newRecord.Stability,
                    Remarks = newRecord.Remarks,
                    MltId = newRecord.MltId,
                    LabId = newRecord.LabId
                };

                await _context.InstrumentalQualityControls.AddAsync(newQCRecord);

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateInstrumentalQualityControlRecord/{id}")]
        public async Task<IActionResult> UpdateInstrumentalQualityControlRecord([FromRoute] String id, [FromBody]UpdateInstrumentalQC instrumentalQC) {
            try {
                var qcRecord = await _context.InstrumentalQualityControls.FindAsync(id);

                if (qcRecord == null) {
                    return NotFound();
                }

                qcRecord.DateTime = instrumentalQC.DateTime;
                qcRecord.InstrumentId = instrumentalQC.InstrumentId;
                qcRecord.TemperatureFluctuation = instrumentalQC.TemperatureFluctuation;
                qcRecord.PressureGradient = instrumentalQC.PressureGradient;
                qcRecord.Timer = instrumentalQC.Timer;
                qcRecord.Sterility = instrumentalQC.Sterility;
                qcRecord.Stability = instrumentalQC.Stability;
                qcRecord.Remarks = instrumentalQC.Remarks;

                await _context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteInstrumentalQualityControlRecord/{id}")]
        public async Task<IActionResult> DeleteInstrumentalQualityControlRecord([FromRoute] String id) {
            try {
                var qcRecord = await _context.InstrumentalQualityControls.FindAsync(id);

                if (qcRecord == null) {
                    return NotFound();
                }

                _context.InstrumentalQualityControls.Remove(qcRecord);

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Instrumental Quality Control Record deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
