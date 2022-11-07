using Bendrabutis.Auth;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Authorize(Roles = DormitoryRoles.Owner)]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.Get(id);
            return user == null ? NotFound($"User with specified id = {id} was not found") : Ok(user);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, string? username, string? email, string? role)
        {
            return await _userService.Update(User, id, username, email, role)
                ? Ok("User updated")
                : BadRequest("Failed to update the user. Check parameters.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await _userService.Delete(User, id)
                ? Ok("User removed.")
                : BadRequest("Failed to remove the user. User was not found or you don't have sufficient permissions.");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangePassword(string id, string oldPass, string newPass)
        {
            return await _userService.ChangePassword(id, oldPass, newPass)
                ? Ok("Password changed.")
                : BadRequest("Failed to change password.");
        }
    }
}
