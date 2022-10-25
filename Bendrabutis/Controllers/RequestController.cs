using Bendrabutis.Models.Enums;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Route("api/Requests")]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _requestService.GetRequests());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var req = await _requestService.Get(id);
            return req == null ? NotFound($"Request with specified id = {id} was not found") : Ok(req);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, RequestType type, string? description)
        {
            var user = await _requestService.GetUser(userId);
            if (user == null) return NotFound($"User with id = {userId} was not found");
            return await _requestService.Create(user, type, description)
                ? CreatedAtAction("Create", "Request created.")
                : BadRequest($"User with id = {userId} was not found");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, RequestType? type, string? description)
        {
            if (type == null && description == null) return BadRequest("Type and description were unspecified");

            return await _requestService.Update(id, type, description)
                ? Ok()
                : NotFound($"Request with id = {id} was not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _requestService.Remove(id)
                ? Ok()
                : NotFound($"Request with specified id = {id} was not found");
        }
    }
}
