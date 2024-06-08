using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Models.DTOs.SurgicalInventoryItems;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.FirebaseStrorage;
using Project_v1.Services.QRGeneratorService;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using System.Diagnostics;
using System.Net;
using Project_v1.Services.Filtering;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Services.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Firebase.Auth;
using Project_v1.Services.ReportService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SurgicalInventoryController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;
        private readonly IQRGenerator _qrGenerator;
        private readonly IStorageService _storageService;
        private readonly IFilter _filter;
        public readonly IReportService _reportService;
        private readonly InventoryOperationsLogger _inventoryLogger;

        public SurgicalInventoryController(ApplicationDBContext context,
                                          IIdGenerator idGenerator,
                                          UserManager<SystemUser> userManager,
                                          IQRGenerator qRGenerator,
                                          IStorageService storageService,
                                          IFilter filter,
                                          IReportService reportService,
                                          InventoryOperationsLogger logger) {
            _context = context;
            _idGenerator = idGenerator;
            _userManager = userManager;
            _qrGenerator = qRGenerator;
            _storageService = storageService;
            _filter = filter;
            _inventoryLogger = logger;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("GetSurgicalCategories")]
        [Authorize]
        public async Task<IActionResult> GetSurgicalCategories([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Query Object cannot be null!" });
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

                var surgicalCategories = _context.SurgicalCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.SurgicalCategoryID,
                        c.SurgicalCategoryName,
                        ItemCount = c.SurgicalInventories.Count,
                        c.LabId
                    });

                var filteredResult = await _filter.Filtering(surgicalCategories, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSurgicalInventoryItem")]
        [Authorize]
        public async Task<IActionResult> GetSurgicalInventoryItem(String itemId) {
            try {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var item = await _context.SurgicalInventory
                    .Where(item => item.SurgicalInventoryID == itemId)
                    .Select(item => new {
                        item.ItemName,
                        item.IssuedDate,
                        DurationOfInventory = (today.DayNumber - item.IssuedDate.DayNumber),
                        item.IssuedBy,
                        item.Quantity,
                        item.SurgicalCategory.SurgicalCategoryName,
                        item.Remarks
                    })
                    .FirstOrDefaultAsync();

                if (item == null) {
                    return NotFound();
                }

                return Ok(item);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSurgicalInventoryItems")]
        [Authorize]
        public async Task<IActionResult> GetSurgicalInventoryItems([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Query Object cannot be null!" });
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

                var category = await _context.SurgicalCategory.FindAsync(query.CategoryId);

                if (category == null) {
                    return NotFound();
                }

                var categoryList = await _context.SurgicalCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.SurgicalCategoryID
                    })
                    .ToListAsync();

                var today = DateOnly.FromDateTime(DateTime.Now);

                if (!categoryList.Any(c => c.SurgicalCategoryID == query.CategoryId)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category does not belong to the lab!" });
                }

                var surgicalItems = _context.SurgicalInventory
                    .Where(items => items.SurgicalCategoryID == query.CategoryId)
                    .Select(items => new {
                        items.SurgicalInventoryID,
                        items.ItemName,
                        items.IssuedDate,
                        items.IssuedBy,
                        items.Quantity,
                        items.SurgicalCategory.SurgicalCategoryID,
                        items.SurgicalCategory.SurgicalCategoryName,
                        DurationOfInventory = (today.DayNumber - items.IssuedDate.DayNumber),
                        items.Remarks,
                        items.ItemQR,
                        items.SurgicalCategory.LabId
                    });

                var filteredResult = await _filter.Filtering(surgicalItems, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSurgicalInventoryQR")]
        [Authorize]
        public async Task<IActionResult> GetSurgicalInventoryQR(String itemId) {
            try {
                var item = await _context.SurgicalInventory.FindAsync(itemId);

                if (item == null) {
                    return NotFound();
                }

                var url = item.ItemQR;

                byte[] fileBytes = await _storageService.DownloadFile(url, itemId);

                return File(fileBytes, "application/pdf", itemId);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetItemIssuingReport")]
        public async Task<IActionResult> GetItemIssuingReport([FromQuery] String MltId, [FromQuery] int Year, [FromQuery] int Month) {
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

                var issuedItems = _context.SurgicalInventory
                    .Include(item => item.IssuedItems)
                    .Where(item => item.IssuedDate.Year <= Year && item.IssuedDate.Month <= Month)
                    .Select(s => new ItemIssuingReport {
                        ItemName = s.ItemName,
                        SurgicalCategory = s.SurgicalCategory.SurgicalCategoryName,
                        InitialQuantity = s.Quantity - s.IssuedItems
                            .Where(i => i.IssuedDate.Month >= Month)
                            .Sum(i => i.AddedQuantity) + s.IssuedItems
                            .Where(i => i.IssuedDate.Month >= Month)
                            .Sum(i => i.IssuedQuantity),
                        IssuedInMonth = s.IssuedItems
                            .Where(i => i.IssuedDate.Year == Year && i.IssuedDate.Month == Month)
                            .Sum(i => i.IssuedQuantity),
                        AddedInMonth = s.IssuedItems
                            .Where(i => i.IssuedDate.Year == Year && i.IssuedDate.Month == Month)
                            .Sum(i => i.AddedQuantity),
                        RemainingQuantity = s.Quantity - s.IssuedItems
                            .Where(i => i.IssuedDate.Month >= Month)
                            .Sum(i => i.AddedQuantity) + s.IssuedItems
                            .Where(i => i.IssuedDate.Month >= Month)
                            .Sum(i => i.IssuedQuantity) - s.IssuedItems
                            .Where(i => i.IssuedDate.Year == Year && i.IssuedDate.Month == Month)
                            .Sum(i => i.IssuedQuantity) + s.IssuedItems
                            .Where(i => i.IssuedDate.Year == Year && i.IssuedDate.Month == Month)
                            .Sum(i => i.AddedQuantity)
                    })
                    .ToList();

                var report = _reportService.GenerateItemIssuingReport(issuedItems, Year, Month);

                return File(report, "application/pdf", "ItemIssuingReport.pdf");
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddSurgicalCategory")]
        [Authorize]
        public async Task<IActionResult> AddSurgicalCategory([FromBody] AddSurgicalCategory category) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (await _context.SurgicalCategory.AnyAsync(c => c.SurgicalCategoryName == category.SurgicalCategoryName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Category already exists!" });
                }

                var newCategory = new SurgicalCategory {
                    SurgicalCategoryID = _idGenerator.GenerateSurgicalCatagoryId(),
                    SurgicalCategoryName = category.SurgicalCategoryName,
                    LabId = category.LabId
                };

                await _context.SurgicalCategory.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Added Surgical Category: {newCategory.SurgicalCategoryID} in Lab: {category.LabId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Catagory Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddSurgicalInventoryItem")]
        [Authorize]
        public async Task<IActionResult> AddSurgicalInventoryItem([FromBody] NewSurgicalItem newSurgicalItem) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (await _context.SurgicalInventory.AnyAsync(c => c.ItemName == newSurgicalItem.ItemName)) {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Item Name exists!" });
                }

                if (newSurgicalItem.IssuedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new { Message = "Issued Date cannot be in the future!" });
                }

                var surgicalInventoryId = _idGenerator.GenerateSurgicalInventoryId();

                byte[] QRCode = _qrGenerator.GenerateSurgicalInventoryQRCode(newSurgicalItem.SurgicalCategoryID, surgicalInventoryId);

                var QRurl = await _storageService.UploadQRCode(new MemoryStream(QRCode), surgicalInventoryId);

                var surgicalInventoryItem = new SurgicalInventory {
                    SurgicalInventoryID = surgicalInventoryId,
                    ItemName = newSurgicalItem.ItemName,
                    IssuedDate = newSurgicalItem.IssuedDate,
                    IssuedBy = newSurgicalItem.IssuedBy,
                    Quantity = newSurgicalItem.Quantity,
                    Remarks = newSurgicalItem.Remarks,
                    ItemQR = QRurl,
                    SurgicalCategoryID = newSurgicalItem.SurgicalCategoryID
                };

                await _context.SurgicalInventory.AddAsync(surgicalInventoryItem);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Added Surgical Inventory Item: {surgicalInventoryId} to {newSurgicalItem.SurgicalCategoryID} Category || Quantity: {newSurgicalItem.Quantity} in Lab: {newSurgicalItem.LabId} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("IssueItem")]
        [Authorize]
        public async Task<IActionResult> IssueItem([FromBody] IssueItem issueItem) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var item = await _context.SurgicalInventory.FindAsync(issueItem.ItemId);

                if (item == null) {
                    return NotFound();
                }

                if (item.Quantity < issueItem.Quantity) {
                    return BadRequest(new { Message = "Not enough quantity available!" });
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                item.Quantity -= issueItem.Quantity;

                var issuedItem = new IssuedItem {
                    IssuedItemID = _idGenerator.GenerateIssuedItemId(),
                    IssuedQuantity = issueItem.Quantity,
                    IssuedBy = issueItem.IssuedBy,
                    IssuedDate = today,
                    Remarks = issueItem.Remarks,
                    SurgicalInventoryID = issueItem.ItemId
                };

                await _context.IssuedItems.AddAsync(issuedItem);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Issued {issueItem.Quantity} units of {issueItem.ItemId} item in Surgical Inventory by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Issued Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch]
        [Route("AddQuantity/{id}")]
        [Authorize]
        public async Task<IActionResult> AddQuantity([FromRoute] string id, [FromBody] IssueItem addQuantity) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var item = await _context.SurgicalInventory.FindAsync(id);

                if (item == null) {
                    return NotFound();
                }
                var today = DateOnly.FromDateTime(DateTime.Now);

                item.Quantity += addQuantity.Quantity;

                var addedItem = new IssuedItem {
                    IssuedItemID = _idGenerator.GenerateIssuedItemId(),
                    AddedQuantity = addQuantity.Quantity,
                    IssuedBy = addQuantity.IssuedBy,
                    IssuedDate = today,
                    Remarks = addQuantity.Remarks,
                    SurgicalInventoryID = addQuantity.ItemId
                };

                await _context.IssuedItems.AddAsync(addedItem);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Added {addQuantity.Quantity} units of {id} item in Surgical Inventory by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Quantity Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSurgicalCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGeneralCategory([FromRoute] string id, [FromBody] UpdateSurgicalCategory updatedCategory) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var surgicalCategory = await _context.SurgicalCategory.FindAsync(id);

                if (surgicalCategory == null) {
                    return NotFound();
                }

                if (surgicalCategory.SurgicalCategoryName != updatedCategory.SurgicalCategoryName) {
                    if (await _context.SurgicalCategory.AnyAsync(c => c.SurgicalCategoryName == updatedCategory.SurgicalCategoryName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Category already exists!" });
                    }
                }

                surgicalCategory.SurgicalCategoryName = updatedCategory.SurgicalCategoryName;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Update Surgical Category: {id} || Surgical Category Name: {updatedCategory.SurgicalCategoryName} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Category Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSurgicalInventoryItem/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSurgicalInventoryItem([FromRoute] string id, [FromBody] UpdateSurgicalItem updatedItem) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var surgicalInventoryItem = await _context.SurgicalInventory.FindAsync(id);

                if (surgicalInventoryItem == null) {
                    return NotFound();
                }

                if (surgicalInventoryItem.ItemName != updatedItem.ItemName) {
                    if (await _context.SurgicalInventory.AnyAsync(c => c.ItemName == updatedItem.ItemName)) {
                        return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Item already exists!" });
                    }
                }

                if (updatedItem.IssuedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new { Message = "Issued Date cannot be in the future!" });
                }

                if (!await _storageService.DeleteQRCode(id)) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting QR Code!");
                }

                byte[] QRCode = _qrGenerator.GenerateSurgicalInventoryQRCode(updatedItem.SurgicalCategoryID, id);

                var QRurl = await _storageService.UploadQRCode(new MemoryStream(QRCode), id);

                surgicalInventoryItem.ItemName = updatedItem.ItemName;
                surgicalInventoryItem.IssuedDate = updatedItem.IssuedDate;
                surgicalInventoryItem.IssuedBy = updatedItem.IssuedBy;
                surgicalInventoryItem.Quantity = updatedItem.Quantity;
                surgicalInventoryItem.Remarks = updatedItem.Remarks;
                surgicalInventoryItem.ItemQR = QRurl;
                surgicalInventoryItem.SurgicalCategoryID = updatedItem.SurgicalCategoryID;

                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Updated Surgical Inventory Item: {id} in {updatedItem.SurgicalCategoryID} Category || Quantity: {updatedItem.Quantity} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSurgicalCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSurgicalCategory([FromRoute] string id) {
            try {
                var surgicalCategory = await _context.SurgicalCategory.FindAsync(id);

                if (surgicalCategory == null) {
                    return NotFound();
                }

                _context.SurgicalCategory.Remove(surgicalCategory);
                await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Deleted Surgical Category: {id} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Category Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSurgicalInventoryItem/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSurgicalInventoryItem([FromRoute] string id) {
            try {
                var surgicalInventoryItem = await _context.SurgicalInventory.FindAsync(id);

                if (surgicalInventoryItem == null) {
                    return NotFound();
                }

                if (await _storageService.DeleteQRCode(id)) {
                    _context.SurgicalInventory.Remove(surgicalInventoryItem);
                    await _context.SaveChangesAsync();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _inventoryLogger.LogInformation($"Deleted Surgical Inventory Item: {id} by: {userId}");

                return Ok(new Response { Status = "Success", Message = "Item Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
