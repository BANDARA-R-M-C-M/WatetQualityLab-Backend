using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models.Login;
using Project_v1.Models.Response;
using Project_v1.Models.Signup;
using Project_v1.Models.Users;
using Project_v1.TokenService;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public UserController(ApplicationDBContext context,
                              UserManager<SystemUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              ITokenService tokenService) {

            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]Login loginUser) {
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

                return Ok(new { UserName = existingUser.UserName,
                                Role = role[0],
                                Token = _tokenService.CreateToken(existingUser) });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }   

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody]Signup registeredUser) {
            try {
                if (registeredUser == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid user data!" });
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == registeredUser.UserName);
                var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registeredUser.Email);
                if (existingUser != null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "UserName already exists!" });
                } else if(existingEmail != null) {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "Email already exists!" });
                }

                var newUser = new SystemUser {
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
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role does not exist!" });
                }

                await _userManager.AddToRoleAsync(newUser, registeredUser.Role);

                return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "User created successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("getmlts")]
        public async Task<IActionResult> GetMLTs() {
            try {
                var mlts = await _userManager.GetUsersInRoleAsync("MLT");
                var usernames = mlts.Select(m => m.UserName);
                return Ok(usernames);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("getphis")]
        public async Task<IActionResult> GetPHIs() {
            try {
                var phis = await _userManager.GetUsersInRoleAsync("PHI");
                var usernames = phis.Select(p => p.UserName);
                return Ok(usernames);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpGet]
        [Route("getmohsupervisors")]
        public async Task<IActionResult> GetAdmins() {
            try {
                var mohsupervisors = await _userManager.GetUsersInRoleAsync("MOH_Supervisor");
                var usernames = mohsupervisors.Select(m => m.UserName);
                return Ok(usernames);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpPut]
        [Route("updateuser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] String id,[FromBody]SystemUser updatedUser) {
            try {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;

                await _userManager.UpdateAsync(user);

                return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }

        [HttpDelete]
        [Route("deleteuser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] String id) {
            try {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }

                await _userManager.DeleteAsync(user);

                return Ok(new Response { Status = "Success", Message = "User deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred while processing your request." + e });
            }
        }
    }
}