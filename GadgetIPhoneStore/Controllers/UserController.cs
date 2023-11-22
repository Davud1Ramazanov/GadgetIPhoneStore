using GadgetIPhoneStore.Models;
using GadgetIPhoneStore.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GadgetIPhoneStore.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> RegAdmin([FromBody] Register model)
        {
            var userEx = await _userManager.FindByNameAsync(model.UserName);
            if (userEx != null) return StatusCode(StatusCodes.Status500InternalServerError, "Admin in db already");

            IdentityUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded) { return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!"); }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok("Admin added!");
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateAccount([FromQuery] string id, Register model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var checkUserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value;
            if (user == null)
            {
                return NotFound();
            }

            if (checkUserName.Equals(user.UserName))
            {
                return Forbid("You can't change your own account, you need another profile.");
            }

            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAccount(Register model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var checkUserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value;
            if (user == null)
            {
                return NotFound();
            }

            if (checkUserName.Equals(user.UserName))
            {
                return Forbid("You can't change your own account, you need another profile.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> SelectAccount()
        {
            var user = await _userManager.Users.ToListAsync();

            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }
    }
}