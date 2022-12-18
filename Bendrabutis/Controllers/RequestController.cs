using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bendrabutis.Models.Enums;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;
using Bendrabutis.Auth;
using Bendrabutis.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Authorize]
    [EnableCors("CorsApi")]
    [Route("api/Requests")]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Gets all user specific or all requests based on user's authorization
        /// </summary>
        /// <returns>List of request based on authorization filter</returns>
        /// <response code="200">List of requests</response>
        /// <response code="401">User is unauthorized</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _requestService.GetRequests(User));
        }

        /// <summary>
        /// Gets a specific request with its information
        /// </summary>
        /// <param name="id">Request's id</param>
        /// <returns>Found request or error message</returns>
        /// <response code="200">Found request</response>
        /// <response code="404">Request with specified id was not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var req = await _requestService.Get(User, id);
            return req == null ? NotFound($"Request with specified id = {id} was not found") : Ok(req);
        }

        /// <summary>
        /// Creates a new request
        /// </summary>
        /// <param name="newPostDto">Request type and description</param>
        /// <returns>Success or error message</returns>
        /// <response code="201">Request created</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="400">User was not found</response>
        [HttpPost]
        public async Task<IActionResult> Create(NewPostDto newPostDto)
        {
            return await _requestService.Create(User.FindFirstValue(JwtRegisteredClaimNames.Sub), newPostDto)
                ? CreatedAtAction("Create", "Request created.")
                : BadRequest($"User with id = {User.FindFirstValue(JwtRegisteredClaimNames.Sub)} was not found");
        }

        /// <summary>
        /// Updates a specific request
        /// </summary>
        /// <param name="id">Request's id</param>
        /// <param name="type">Optional request type to set</param>
        /// <param name="description">Optional request description to set</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Request updated successfully</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="400">No parameters were used in API call</response>
        /// <response code="404">Request with specified was not found</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, RequestType? type, string? description)
        {
            if (type == null && description == null) return BadRequest("Type and description were unspecified");
            var a = User;
            return await _requestService.Update(User, id, type, description)
                ? Ok()
                : NotFound($"Request with id = {id} was not found");
        }

        /// <summary>
        /// Deletes a specific request
        /// </summary>
        /// <param name="id">Request's id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Request deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Request with specified was not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _requestService.Remove(id)
                ? Ok()
                : NotFound($"Request with specified id = {id} was not found");
        }
    }
}