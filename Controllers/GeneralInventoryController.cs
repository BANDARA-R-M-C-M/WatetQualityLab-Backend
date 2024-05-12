using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.GeneralInventoryItems;
using Project_v1.Services.IdGeneratorService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralInventoryController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly IIdGenerator _idGenerator;

        public GeneralInventoryController(ApplicationDBContext context,
                                          IIdGenerator idGenerator) {
            _context = context;
            _idGenerator = idGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> GetGeneralInventoryItems(String LabId) {
            try {
                var lab = await _context.Labs.FindAsync(LabId);

                if (lab == null) {
                    return NotFound();
                }

                var generalInventoryItems = await _context.GeneralInventory
                    .Where(items => items.LabId == LabId)
                    .Select(items => new {
                        items.GeneralInventoryID,
                        items.ItemName,
                        items.IssuedDate,
                        items.IssuedBy,
                        items.DurationOfInventory,
                        items.Remarks
                    })
                    .ToListAsync();

                return Ok(generalInventoryItems);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewGeneralInventoryItem(NewGeneralItem newGeneralItem) {
            try {

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var generalInventoryItem = new GeneralInventory {
                    GeneralInventoryID = _idGenerator.GenerateGeneralInventoryId(),
                    ItemName = newGeneralItem.ItemName,
                    IssuedDate = newGeneralItem.IssuedDate,
                    IssuedBy = newGeneralItem.IssuedBy,
                    DurationOfInventory = newGeneralItem.DurationOfInventory,
                    Remarks = newGeneralItem.Remarks,
                    LabId = newGeneralItem.LabId
                };

                _context.GeneralInventory.Add(generalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok(generalInventoryItem);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGeneralInventoryItem(string id, NewGeneralItem newGeneralItem) {
            try {
                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                generalInventoryItem.ItemName = newGeneralItem.ItemName;
                generalInventoryItem.IssuedDate = newGeneralItem.IssuedDate;
                generalInventoryItem.IssuedBy = newGeneralItem.IssuedBy;
                generalInventoryItem.DurationOfInventory = newGeneralItem.DurationOfInventory;
                generalInventoryItem.Remarks = newGeneralItem.Remarks;
                generalInventoryItem.LabId = newGeneralItem.LabId;

                await _context.SaveChangesAsync();

                return Ok(generalInventoryItem);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralInventoryItem(string id) {
            try {
                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                _context.GeneralInventory.Remove(generalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
