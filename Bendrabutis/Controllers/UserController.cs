using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
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

        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string fullName, string phoneNumber)
        {
            return await _userService.Create(username, password, fullName, phoneNumber)
                ? CreatedAtAction("Create", $"User with username {username} created.")
                : Conflict($"User with name = {username} already exists");
        }

        [HttpPost("AssignRoom")]
        public async Task<IActionResult> AssignRoom(int id, int roomId)
        {
            var room = await _userService.GetRoom(roomId);
            if (room == null) return NotFound($"Room with specified id = {roomId} was not found");

            return await _userService.AssignRoom(id, room)
                ? Ok()
                : BadRequest($"Specified user is already a resident in this room or was not found");
        }

        [HttpPost("RemoveFromRoom")]
        public async Task<IActionResult> RemoveFromRoom(int id)
        {
            return await _userService.RemoveFromRoom(id)
                ? Ok()
                : NotFound($"User with specified id = {id} was not found");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, string? username, string? password, string? fullName,
            string? phoneNumber)
        {
            if (username == null && password == null && fullName == null && phoneNumber == null)
                return BadRequest("No properties to update were unspecified");

            return await _userService.Update(id, username, password, fullName, phoneNumber)
                ? Ok()
                : NotFound($"User with specified id = {id} was not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _userService.Delete(id) ? Ok() : NotFound($"User with specified id = {id} was not found");
        }
    }
}
