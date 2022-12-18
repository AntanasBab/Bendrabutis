using Bendrabutis.Auth;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Authorize]
    [EnableCors("CorsApi")]
    [Route("api/Dormitories")]
    public class DormitoryController : ControllerBase
    {
        private readonly DormitoryService _dormitoryService;

        public DormitoryController(DormitoryService dormitoryService)
        {
            _dormitoryService = dormitoryService;
        }

        /// <summary>
        /// Gets all available dormitories.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Returns all dormitories.
        /// </para>
        /// </remarks>
        /// <response code="200">List of dormitories.</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dormitoryService.GetAllDormitories());
        }

        /// <summary>
        /// Fetches a specific dormitory.
        /// </summary>
        /// <param name="id">Dormitory's id.</param>
        /// <remarks>
        /// <para>
        /// Returns dormitory with its information.
        /// </para>
        /// </remarks>
        /// <response code="200">Found dormitory.</response>
        /// <response code="400">Dormitory with specified id was not found.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var dormitory = await _dormitoryService.GetDormitory(id);
            return dormitory != null ? Ok(dormitory) : NotFound($"Dormitory with id = {id} was not found.");
        }

        /// <summary>
        /// Creates a new dormitory.
        /// </summary>
        /// <param name="name">Dormitory name.</param>
        /// <param name="address">Dormitory's address.</param>
        /// <param name="roomCapacity">Dormitory's room capacity.</param>
        /// <para>
        /// Success/failure message
        /// </para>
        /// <response code="201">Dormitory created successfully.</response>
        /// <response code="400">Invalid parameters were entered.</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="409">Dormitory with the same name already exists.</response>
        [HttpPost]
        [Authorize(Roles = $"{DormitoryRoles.Admin}, {DormitoryRoles.Owner}")]
        public async Task<IActionResult> Create(string name, string address, int? roomCapacity)
        {
            if (roomCapacity == null) return BadRequest("Room capacity is required.");

            return await _dormitoryService.CreateDormitory(name, address, roomCapacity.Value)
                ? CreatedAtAction("Create", $"Dormitory with name {name} created.")
                : Conflict("Dormitory with specified name already exists.");
        }


        /// <summary>
        /// Updates a specific dormitory.
        /// </summary>
        /// <param name="id">Dormitory id</param>
        /// <param name="name">Optional dormitory name to set</param>
        /// <param name="address">Optional dormitory address to set</param>
        /// <param name="roomCapacity">Optional dormitory room capacity to set</param>
        /// <returns>Success/failure message</returns>
        /// <response code="200">Dormitory updated successfully</response>
        /// <response code="400">No parameters were entered</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="404">Dormitory not found </response>
        [HttpPatch("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Update(int id, string? name = null, string? address = null,
            int? roomCapacity = null)
        {
            if (name == null && address == null && roomCapacity == null) return BadRequest();

            return await _dormitoryService.UpdateDormitory(id, name, address, roomCapacity)
                ? Ok()
                : NotFound($"Dormitory with id = {id} was not found.");
        }

        /// <summary>
        /// Deletes a specific dormitory.
        /// </summary>
        /// <param name="id">Dormitory id</param>
        /// <returns>Success/failure message</returns>
        /// <response code="200">Dormitory deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="404">Dormitory not found </response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _dormitoryService.DeleteDormitory(id)
                ? Ok($"Dormitory with id = {id} removed.")
                : NotFound($"Dormitory with id = {id} was not found.");
        }
    }
}