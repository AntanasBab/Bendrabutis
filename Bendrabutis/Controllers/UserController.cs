using Bendrabutis.Models;
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

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserLogin request)
        //{
        //    throw new NotImplementedException();
        //}

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
