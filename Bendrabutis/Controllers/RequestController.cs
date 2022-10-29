using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bendrabutis.Models.Enums;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Bendrabutis.Auth;
using Bendrabutis.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Authorize]
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
        [Authorize(Roles =  DormitoryRoles.Visitor)]
        public async Task<IActionResult> Create(NewPostDto newPostDto)
        {
            return await _requestService.Create(User.FindFirstValue(JwtRegisteredClaimNames.Sub), newPostDto)
                ? CreatedAtAction("Create", "Request created.")
                : BadRequest($"User with id = {User.FindFirstValue(JwtRegisteredClaimNames.Sub)} was not found");
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
