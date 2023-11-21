using GadgetIPhoneStore.Models;
using GadgetIPhoneStore.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GadgetIPhoneStore.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<IdentityUser> _userManager;

        public AccountController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("CreateAccount")]
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
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromQuery]string id, Register model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            
            user.UserName = model.UserName;

            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] hasedPassword = sHA256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                user.PasswordHash = Convert.ToBase64String(hasedPassword);
            }

            user.Email = model.Email;
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(Register model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("SelectAccounts")]
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
