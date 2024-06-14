using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.Logging;
using Project_v1.Services.QRGeneratorService;
using Project_v1.Services.ReportService;
using System.Security.Claims;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralInventoryController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IQRGenerator _qrGenerator;
        private readonly IFilter _filter;
        private readonly IReportService _reportService;
        private readonly InventoryOperationsLogger _inventoryLogger;

        public GeneralInventoryController(ApplicationDBContext context,
                                          IIdGenerator idGenerator,
                                          UserManager<SystemUser> userManager,
                                          IQRGenerator qRGenerator,
                                          IFilter filter,
                                          IReportService reportService,
                                          InventoryOperationsLogger inventoryLogger) {
            _context = context;
            _idGenerator = idGenerator;
            _userManager = userManager;
            _qrGenerator = qRGenerator;
            _filter = filter;
            _reportService = reportService;
            _inventoryLogger = inventoryLogger;
        }

        [HttpGet]
        [Route("GetGeneralCategories")]
        [Authorize]
        public async Task<ActionResult> GetGeneralCategories([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound();
                }

                var labId = mlt.LabID;

                var lab = await _context.Labs.FindAsync(labId);

                if (lab == null) {
                    return NotFound("No Laboratory assigned!");
                }

                var generalCategories = _context.GeneralCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.GeneralCategoryID,
                        c.GeneralCategoryName,
                        ItemCount = c.GeneralInventories.Count(),
                        c.LabId
                    });

                var filteredResult = await _filter.Filtering(generalCategories, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetGeneralInventoryItem")]
        [Authorize]
        public async Task<ActionResult> GetGeneralInventoryItem(String itemId) {
            try {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var generalInventoryItem = await _context.GeneralInventory
                    .Where(item => item.GeneralInventoryID == itemId)
                    .Select(item => new {
                        item.ItemName,
                        item.IssuedDate,
                        DurationOfInventory = (today.DayNumber - item.IssuedDate.DayNumber),
                        item.IssuedBy,
                        item.GeneralCategory.GeneralCategoryName,
                        item.Remarks
                    })
                    .FirstOrDefaultAsync();

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                return Ok(generalInventoryItem);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetGeneralInventoryItems")]
        [Authorize]
        public async Task<ActionResult> GetGeneralInventoryItems([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null || query.CategoryId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound();
                }

                var labId = mlt.LabID;

                var lab = await _context.Labs.FindAsync(labId);

                if (lab == null) {
                    return NotFound();
                }

                var category = await _context.GeneralCategory.FindAsync(query.CategoryId);

                if (category == null) {
                    return NotFound();
                }

                var categoryList = await _context.GeneralCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.GeneralCategoryID
                    })
                    .ToListAsync();

                var today = DateOnly.FromDateTime(DateTime.Now);

                if (!categoryList.Any(c => c.GeneralCategoryID == query.CategoryId)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category does not belong to the lab!" });
                }

                var generalInventoryItems = _context.GeneralInventory
                        .Where(items => items.GeneralCategoryID == query.CategoryId)
                        .Select(items => new {
                            items.GeneralInventoryID,
                            items.ItemName,
                            items.IssuedDate,
                            items.IssuedBy,
                            items.GeneralCategory.GeneralCategoryID,
                            items.GeneralCategory.GeneralCategoryName,
                            DurationOfInventory = (today.DayNumber - items.IssuedDate.DayNumber),
                            items.Remarks,
                            items.GeneralCategory.LabId
                        });

                var filteredResult = await _filter.Filtering(generalInventoryItems, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetGeneralInventoryQR")]
        [Authorize]
        public async Task<IActionResult> GetSurgicalInventoryQR(String itemId) {
            try {
                var item = await _context.GeneralInventory.FindAsync(itemId);

                if (item == null) {
                    return NotFound();
                }

                byte[] qrCode = _qrGenerator.GenerateGeneralInventoryQRCode(item.GeneralCategoryID, itemId);

                return File(qrCode, "application/pdf", itemId);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetInventoryDurationDetails")]
        [Authorize]
        public async Task<IActionResult> GetInventoryDurationDetails([FromQuery] QueryObject query, [FromQuery] int Year) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound("No Lab assigned.");
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                var inventories = _context.GeneralInventory
                    .Include(g => g.GeneralCategory)
                    .Where(g => g.IssuedDate.Year <= Year)
                    .Select(g => new GeneralInventoryReport {
                        ItemName = g.ItemName,
                        IssuedDate = g.IssuedDate,
                        Duration = (today.DayNumber - g.IssuedDate.DayNumber),
                        IssuedBy = g.IssuedBy,
                        Remarks = g.Remarks,
                        GeneralCategoryName = g.GeneralCategory.GeneralCategoryName
                    });

                var filteredResult = await _filter.Filtering(inventories, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetInventoryDurationReport")]
        [Authorize]
        public async Task<IActionResult> GetInventoryDurationReport([FromQuery] String MltId, [FromQuery] int Year) {
            try {
                if (MltId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(MltId);

                if (mlt == null) {
                    return NotFound($"User with username '{MltId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{MltId}' does not have a Lab assigned.");
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                var inventories = await _context.GeneralInventory
                    .Include(g => g.GeneralCategory)
                    .Where(g => g.IssuedDate.Year <= Year)
                    .Select(g => new GeneralInventoryReport {
                        ItemName = g.ItemName,
                        IssuedDate = g.IssuedDate,
                        Duration = (today.DayNumber - g.IssuedDate.DayNumber),
                        IssuedBy = g.IssuedBy,
                        Remarks = g.Remarks,
                        GeneralCategoryName = g.GeneralCategory.GeneralCategoryName
                    })
                    .ToListAsync();

                byte[] report = _reportService.GenerateInventoryDurationReport(inventories, Year);

                return File(report, "application/pdf", "InventoryDurationReport-" + Year + ".pdf");
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost]
        [Route("AddGeneralCategory")]
        [Authorize]
        public async Task<IActionResult> AddGeneralCategory([FromBody] AddGeneralCategory category) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (await _context.GeneralCategory.AnyAsync(c => c.GeneralCategoryName == category.GeneralCategoryName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Category already exists!" });
                }

                var generalDategoryId = _idGenerator.GenerateGeneralCatagoryId();

                var newCategory = new GeneralCategory {
                    GeneralCategoryID = generalDategoryId,
                    GeneralCategoryName = category.GeneralCategoryName,
                    LabId = category.LabId
                };

                _context.GeneralCategory.Add(newCategory);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Added General Category: {generalDategoryId} to  Lab: {category.LabId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Catagory Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddGeneralInventoryItem")]
        [Authorize]
        public async Task<IActionResult> AddGeneralInventoryItem([FromBody] NewGeneralItem newGeneralItem) {
            try {

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (newGeneralItem.IssuedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new { Message = "Issued Date cannot be in the future!" });
                }

                if (await _context.GeneralInventory.AnyAsync(c => c.ItemName == newGeneralItem.ItemName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Item Already exists!" });
                }

                var generalInventoryId = _idGenerator.GenerateGeneralInventoryId();

                var generalInventoryItem = new GeneralInventory {
                    GeneralInventoryID = generalInventoryId,
                    ItemName = newGeneralItem.ItemName,
                    IssuedDate = newGeneralItem.IssuedDate,
                    IssuedBy = newGeneralItem.IssuedBy,
                    Remarks = newGeneralItem.Remarks,
                    GeneralCategoryID = newGeneralItem.GeneralCategoryID
                };

                _context.GeneralInventory.Add(generalInventoryItem);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Added General Inventory Item: {generalInventoryId} to {newGeneralItem.GeneralCategoryID} in Lab: {newGeneralItem.LabId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateGeneralCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGeneralCategory([FromRoute] string id, [FromBody] UpdateGeneralCategory updatedCategory) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var generalCategory = await _context.GeneralCategory.FindAsync(id);

                if (generalCategory == null) {
                    return NotFound();
                }

                if (generalCategory.GeneralCategoryName != updatedCategory.GeneralCategoryName) {
                    if (await _context.GeneralCategory.AnyAsync(c => c.GeneralCategoryName == updatedCategory.GeneralCategoryName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Category already exists!" });
                    }
                }

                generalCategory.GeneralCategoryName = updatedCategory.GeneralCategoryName;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Update General Category: {id} || General Category Name: {updatedCategory.GeneralCategoryName} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Category Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateGeneralInventoryItem/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGeneralInventoryItem([FromRoute] string id, [FromBody] UpdateGeneralItem updateGeneralItem) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                if (generalInventoryItem.ItemName != updateGeneralItem.ItemName) {
                    if (await _context.GeneralInventory.AnyAsync(c => c.ItemName == updateGeneralItem.ItemName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Item already exists!" });
                    }
                }

                generalInventoryItem.ItemName = updateGeneralItem.ItemName;
                generalInventoryItem.IssuedDate = updateGeneralItem.IssuedDate;
                generalInventoryItem.IssuedBy = updateGeneralItem.IssuedBy;
                generalInventoryItem.Remarks = updateGeneralItem.Remarks;
                generalInventoryItem.GeneralCategoryID = updateGeneralItem.GeneralCategoryID;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Update General Inventory Item: {id} in {updateGeneralItem.GeneralCategoryID} Category by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteGeneralCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGeneralCategory([FromRoute] string id) {
            try {
                var generalCategory = await _context.GeneralCategory.FindAsync(id);

                if (generalCategory == null) {
                    return NotFound();
                }

                _context.GeneralCategory.Remove(generalCategory);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Delete General Category: {id} Deleted by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Category Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteGeneralInventoryItem/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGeneralInventoryItem([FromRoute] string id) {
            try {
                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                _context.GeneralInventory.Remove(generalInventoryItem);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Delete General Inventory Item: {id} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
