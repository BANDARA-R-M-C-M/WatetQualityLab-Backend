﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models.GeneralInventoryItems;
using Project_v1.Models;
using Project_v1.Models.Response;
using Project_v1.Models.Users;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Models.SurgicalInventoryItems;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SurgicalInventoryController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IIdGenerator _idGenerator;

        public SurgicalInventoryController(ApplicationDBContext context,
                                          IIdGenerator idGenerator,
                                          UserManager<SystemUser> userManager) {
            _context = context;
            _idGenerator = idGenerator;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetSurgicalCategories")]
        public async Task<IActionResult> GetSurgicalCategories(String mltId) {
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

                var surgicalCategories = await _context.SurgicalCategory
                    .Where(c => c.LabId == labId)
                    .Select(c => new {
                        c.SurgicalCategoryID,
                        c.CategoryName,
                        c.LabId
                    })
                    .ToListAsync();

                return Ok(surgicalCategories);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSurgicalInventoryItems")]
        public async Task<IActionResult> GetSurgicalInventoryItems(String mltId, String categoryId) {
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

                var category = await _context.SurgicalCategory.FindAsync(categoryId);

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

                if (!categoryList.Any(c => c.SurgicalCategoryID == categoryId)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category does not belong to the lab!" });
                }

                var surgicalItems = await _context.SurgicalInventory
                    .Where(items => items.SurgicalCategoryID == categoryId)
                    .Select(items => new {
                        items.SurgicalInventoryID,
                        items.ItemName,
                        items.IssuedDate,
                        items.IssuedBy,
                        items.Quantity,
                        items.SurgicalCategory.SurgicalCategoryID,
                        items.SurgicalCategory.CategoryName,
                        DurationOfInventory = (today.DayNumber - items.IssuedDate.DayNumber),
                        items.Remarks,
                        items.SurgicalCategory.LabId
                    })
                    .ToListAsync();

                return Ok(surgicalItems);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddSurgicalCategory")]
        public async Task<IActionResult> AddSurgicalCategory([FromBody] AddSurgicalCategory category) {
            try {
                if (category == null || !ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (await _context.SurgicalCategory.AnyAsync(c => c.CategoryName == category.CategoryName)) {
                    return BadRequest(new Response { Status = "Error", Message = "Category already exists!" });
                }

                var newCategory = new SurgicalCategory {
                    SurgicalCategoryID = _idGenerator.GenerateSurgicalInventoryId(),
                    CategoryName = category.CategoryName,
                    LabId = category.LabId
                };

                _context.SurgicalCategory.Add(newCategory);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Catagory Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddSurgicalInventoryItem")]
        public async Task<IActionResult> AddSurgicalInventoryItem([FromBody] NewSurgicalItem newSurgicalItem) {
            try {
                if (newSurgicalItem == null || !ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (newSurgicalItem.IssuedDate > DateOnly.FromDateTime(DateTime.Now)) {
                    return BadRequest(new Response { Status = "Error", Message = "Issued Date cannot be in the future!" });
                }

                if (newSurgicalItem.SurgicalCategoryID == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Category ID cannot be null!" });
                }

                if (newSurgicalItem.LabId == null) {
                    return BadRequest(new Response { Status = "Error", Message = "Lab ID cannot be null!" });
                }

                var surgicalInventoryItem = new SurgicalInventory {
                    SurgicalInventoryID = _idGenerator.GenerateSurgicalInventoryId(),
                    ItemName = newSurgicalItem.ItemName,
                    IssuedDate = newSurgicalItem.IssuedDate,
                    IssuedBy = newSurgicalItem.IssuedBy,
                    Quantity = newSurgicalItem.Quantity,
                    Remarks = newSurgicalItem.Remarks,
                    SurgicalCategoryID = newSurgicalItem.SurgicalCategoryID
                };

                _context.SurgicalInventory.Add(surgicalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Added Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSurgicalCategory/{id}")]
        public async Task<IActionResult> UpdateGeneralCategory([FromRoute] string id, [FromBody] UpdateSurgicalCategory updatedCategory) {
            try {
                var surgicalCategory = await _context.SurgicalCategory.FindAsync(id);

                if (surgicalCategory == null) {
                    return NotFound();
                }

                surgicalCategory.CategoryName = updatedCategory.CategoryName;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Category Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSurgicalInventoryItem/{id}")]
        public async Task<IActionResult> UpdateSurgicalInventoryItem([FromRoute] string id, [FromBody] UpdateSurgicalItem updatedItem) {
            try {
                var surgicalInventoryItem = await _context.SurgicalInventory.FindAsync(id);

                if (surgicalInventoryItem == null) {
                    return NotFound();
                }

                surgicalInventoryItem.ItemName = updatedItem.ItemName;
                surgicalInventoryItem.IssuedDate = updatedItem.IssuedDate;
                surgicalInventoryItem.IssuedBy = updatedItem.IssuedBy;
                surgicalInventoryItem.Quantity = updatedItem.Quantity;
                surgicalInventoryItem.Remarks = updatedItem.Remarks;
                surgicalInventoryItem.SurgicalCategoryID = updatedItem.SurgicalCategoryID;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Updated Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSurgicalCategory/{id}")]
        public async Task<IActionResult> DeleteSurgicalCategory([FromRoute] string id) {
            try {
                var surgicalCategory = await _context.SurgicalCategory.FindAsync(id);

                if (surgicalCategory == null) {
                    return NotFound();
                }

                _context.SurgicalCategory.Remove(surgicalCategory);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Category Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSurgicalInventoryItem/{id}")]
        public async Task<IActionResult> DeleteSurgicalInventoryItem([FromRoute] string id) {
            try {
                var surgicalInventoryItem = await _context.SurgicalInventory.FindAsync(id);

                if (surgicalInventoryItem == null) {
                    return NotFound();
                }

                _context.SurgicalInventory.Remove(surgicalInventoryItem);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Item Deleted Successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
