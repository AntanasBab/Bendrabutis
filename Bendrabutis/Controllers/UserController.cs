using Bendrabutis.Auth;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [EnableCors("CorsApi")]
    [Authorize(Roles = DormitoryRoles.Owner)]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">List of all users</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpPost("AssignRoom/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers());
        }

        /// <summary>
        /// Gets a specific user's information
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>A specific user's information</returns>
        /// <response code="200">Specific's user's info</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.Get(id);
            return user == null ? NotFound($"User with specified id = {id} was not found") : Ok(user);
        }

        /// <summary>
        /// Updates a specific user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="username">Optional user's username to set</param>
        /// <param name="email">Optional user's email to set</param>
        /// <param name="role">Optional user's role to set</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User was updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">User with specified id was not found</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, string? username, string? email, string? role)
        {
            return await _userService.Update(User, id, username, email, role)
                ? Ok("User updated")
                : BadRequest("Failed to update the user. Check parameters.");
        }

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">User with specified id was not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await _userService.Delete(User, id)
                ? Ok("User removed.")
                : BadRequest("Failed to remove the user. User was not found or you don't have sufficient permissions.");
        }

        /// <summary>
        /// Changes a user's password
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="oldPass">User's old password</param>
        /// <param name="newPass">User's new password</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">User with specified id was not found</response>
        [HttpPost("{id}")]
        public async Task<IActionResult> ChangePassword(string id, string oldPass, string newPass)
        {
            return await _userService.ChangePassword(id, oldPass, newPass)
                ? Ok("Password changed.")
                : BadRequest("Failed to change password.");
        }
    }
}
