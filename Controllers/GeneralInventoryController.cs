using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using Project_v1.Models.DTOs.Response;
using Project_v1.Services.IdGeneratorService;

namespace Project_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralInventoryController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;

        public GeneralInventoryController(ApplicationDBContext context,
                                          IIdGenerator idGenerator,
                                          UserManager<SystemUser> userManager) {
            _context = context;
            _idGenerator = idGenerator;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetGeneralCategories")]
        public async Task<ActionResult> GetGeneralCategories(String mltId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltId);

                if (mlt == null) {
                    return NotFound();
                }

                var labId = mlt.LabID;

                var lab = await _context.Labs.FindAsync(labId);

                if (lab == null) {
                    return NotFound();
                }

                var generalCategories = await _context.GeneralCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.GeneralCategoryID,
                        c.CategoryName,
                        c.LabId
                    })
                    .ToListAsync();

                return Ok(generalCategories);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetGeneralInventoryItems")]
        public async Task<ActionResult> GetGeneralInventoryItems(String mltId, String categoryId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mltId);

                if (mlt == null) {
                    return NotFound();
                }

                var labId = mlt.LabID;

                var lab = await _context.Labs.FindAsync(labId);

                if (lab == null) {
                    return NotFound();
                }

                var category = await _context.GeneralCategory.FindAsync(categoryId);

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

                if (!categoryList.Any(c => c.GeneralCategoryID == categoryId)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category does not belong to the lab!" });
                }

                var generalInventoryItems = await _context.GeneralInventory
                        .Where(items => items.GeneralCategoryID == categoryId)
                        .Select(items => new {
                            items.GeneralInventoryID,
                            items.ItemName,
                            items.IssuedDate,
                            items.IssuedBy,
                            items.GeneralCategory.GeneralCategoryID,
                            items.GeneralCategory.CategoryName,
                            DurationOfInventory = (today.DayNumber - items.IssuedDate.DayNumber),
                            items.Remarks,
                            items.GeneralCategory.LabId
                        })
                        .ToListAsync();

                return Ok(generalInventoryItems);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddGeneralCategory")]
        public async Task<IActionResult> AddGeneralCategory([FromBody] AddGeneralCategory category) {
            try {
                if (category == null || !ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if(await _context.GeneralCategory.AnyAsync(c => c.CategoryName == category.CategoryName)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category already exists!" });
                }

                var newCategory = new GeneralCategory {
                    GeneralCategoryID = _idGenerator.GenerateGeneralInventoryId(),
                    CategoryName = category.CategoryName,
                    LabId = category.LabId
                };

                _context.GeneralCategory.Add(newCategory);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Catagory Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddGeneralInventoryItem")]
        public async Task<IActionResult> AddGeneralInventoryItem([FromBody] NewGeneralItem newGeneralItem) {
            try {

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (newGeneralItem.IssuedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new Response { Status = "Error", Message = "Issued Date cannot be in the future!" });
                }

                if (newGeneralItem.GeneralCategoryID == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Category ID cannot be null!" });
                }

                if (newGeneralItem.LabId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Lab ID cannot be null!" });
                }

                var generalInventoryItem = new GeneralInventory {
                    GeneralInventoryID = _idGenerator.GenerateGeneralInventoryId(),
                    ItemName = newGeneralItem.ItemName,
                    IssuedDate = newGeneralItem.IssuedDate,
                    IssuedBy = newGeneralItem.IssuedBy,
                    Remarks = newGeneralItem.Remarks,
                    GeneralCategoryID = newGeneralItem.GeneralCategoryID
                };

                _context.GeneralInventory.Add(generalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateGeneralCategory/{id}")]
        public async Task<IActionResult> UpdateGeneralCategory([FromRoute] string id, [FromBody] UpdateGeneralCategory updatedCategory) {
            try {
                var generalCategory = await _context.GeneralCategory.FindAsync(id);

                if (generalCategory == null) {
                    return NotFound();
                }

                generalCategory.CategoryName = updatedCategory.CategoryName;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Category Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateGeneralInventoryItem/{id}")]
        public async Task<IActionResult> UpdateGeneralInventoryItem([FromRoute] string id, [FromBody] UpdateGeneralItem newGeneralItem) {
            try {
                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                generalInventoryItem.ItemName = newGeneralItem.ItemName;
                generalInventoryItem.IssuedDate = newGeneralItem.IssuedDate;
                generalInventoryItem.IssuedBy = newGeneralItem.IssuedBy;
                generalInventoryItem.Remarks = newGeneralItem.Remarks;
                generalInventoryItem.GeneralCategoryID = newGeneralItem.GeneralCategoryID;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteGeneralCategory/{id}")]
        public async Task<IActionResult> DeleteGeneralCategory([FromRoute] string id) {
            try {
                var generalCategory = await _context.GeneralCategory.FindAsync(id);

                if (generalCategory == null) {
                    return NotFound();
                }

                _context.GeneralCategory.Remove(generalCategory);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Category Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteGeneralInventoryItem/{id}")]
        public async Task<IActionResult> DeleteGeneralInventoryItem([FromRoute] string id) {
            try {
                var generalInventoryItem = await _context.GeneralInventory.FindAsync(id);

                if (generalInventoryItem == null) {
                    return NotFound();
                }

                _context.GeneralInventory.Remove(generalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
