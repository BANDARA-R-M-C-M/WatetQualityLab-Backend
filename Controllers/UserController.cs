using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.Login;
using Project_v1.Models.DTOs.Response;
using Project_v1.Models.DTOs.Signup;
using Project_v1.Services.Filtering;
using Project_v1.Services.TokenService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IFilter _filter;

        public UserController(ApplicationDBContext context,
                              UserManager<SystemUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              ITokenService tokenService,
                              IFilter filter) {

            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _filter = filter;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login loginUser) {
            try {
                if (loginUser == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid user data!" });
                }

                var existingUser = await _userManager.FindByNameAsync(loginUser.UserName);
                if (existingUser == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                var validPassword = await _userManager.CheckPasswordAsync(existingUser, loginUser.Password);
                if (!validPassword) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Invalid password!" });
                }

                var role = await _userManager.GetRolesAsync(existingUser);

                string areaId = null;

                if (role[0] == "Mlt") {
                    areaId = existingUser.LabID;
                } else if (role[0] == "Phi") {
                    areaId = existingUser.PHIAreaId;
                } else if (role[0] == "MohSupervisor") {
                    areaId = existingUser.MOHAreaId;
                }

                return Ok(new {
                    Username = existingUser.UserName,
                    UserId = existingUser.Id,
                    Role = role[0],
                    AreaId = areaId,
                    Token = _tokenService.CreateToken(existingUser)
                });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] Signup registeredUser) {
            try {
                if (registeredUser == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid user data!" });
                }

                var existingId = await _context.Users.FirstOrDefaultAsync(u => u.Id == registeredUser.Id);
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == registeredUser.UserName);
                var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registeredUser.Email);

                if (existingId != null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "User ID already exists!" });
                } else if (existingUser != null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "UserName already exists!" });
                } else if (existingEmail != null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Email already exists!" });
                }

                var newUser = new SystemUser {
                    Id = registeredUser.Id,
                    UserName = registeredUser.UserName,
                    Email = registeredUser.Email,
                    PhoneNumber = registeredUser.PhoneNumber
                };

                if (await _roleManager.RoleExistsAsync(registeredUser.Role)) {
                    var createdUser = await _userManager.CreateAsync(newUser, registeredUser.Password);
                    if (!createdUser.Succeeded) {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"User creation failed: {createdUser.Errors.Select(e => e.Description)}" });
                    }
                } else {
                    return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "Role Does Not Exist!" });
                }

                await _userManager.AddToRoleAsync(newUser, registeredUser.Role);

                return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "User created successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getMLTs")]
        public async Task<IActionResult> GetMLTs([FromQuery] QueryObject query) {
            try {
                var mlts = await _userManager.GetUsersInRoleAsync("MLT");

                var mltIds = mlts.Select(m => m.Id);

                var userDetails = _context.Users
                    .Where(m => mltIds
                    .Contains(m.Id))
                    .Select(m => new {
                        m.Id,
                        m.UserName,
                        m.Email,
                        m.PhoneNumber,
                        LabName = m.Lab.LabName ?? "No Lab Assigned"
                    });

                var filteredResult = await _filter.Filtering(userDetails, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getPHIs")]
        public async Task<IActionResult> GetPHIs([FromQuery] QueryObject query) {
            try {
                var phis = await _userManager.GetUsersInRoleAsync("PHI");

                var phiIds = phis.Select(p => p.Id);

                var userDetails = _context.Users
                    .Where(p => phiIds
                    .Contains(p.Id))
                    .Select(p => new {
                        p.Id,
                        p.UserName,
                        p.Email,
                        p.PhoneNumber,
                        PHIName = p.PHIArea.PHIAreaName ?? "No PHI Area Assigned"
                    });

                var filteredResult = await _filter.Filtering(userDetails, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getMOHSupervisors")]
        public async Task<IActionResult> GetMOHSupervisors([FromQuery] QueryObject query) {
            try {
                var mohsupervisors = await _userManager.GetUsersInRoleAsync("MOH_Supervisor");

                var mohids = mohsupervisors.Select(m => m.Id);

                var userDetails = _context.Users
                    .Where(m => mohids
                    .Contains(m.Id))
                    .Select(m => new {
                        m.Id,
                        m.UserName,
                        m.Email,
                        m.PhoneNumber,
                        MOHAreaName = m.MOHArea.MOHAreaName ?? "No MOH Area Assigned"
                    });

                var filteredResult = await _filter.Filtering(userDetails, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] String id, [FromBody] SystemUser updatedUser) {
            try {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                if (updatedUser == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid user data!" });
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] String id) {
            try {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "User deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}