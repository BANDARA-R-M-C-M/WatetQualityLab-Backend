using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models.Response;
using Project_v1.Models.Users;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserassignController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;

        public UserassignController(ApplicationDBContext context, UserManager<SystemUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("mlt-to-lab")]
        public async Task<IActionResult> Assign_mlt_lab(String mlt_id, String labId) {
            try {
                var mlt = await _userManager.FindByIdAsync(mlt_id);

                if (mlt == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                if(labId == null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Lab not found" });
                }

                mlt.LabID = labId;
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Lab assigned successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
    }
}
