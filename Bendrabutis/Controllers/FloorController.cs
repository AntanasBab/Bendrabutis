using Bendrabutis.Auth;
using Bendrabutis.Entities;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Authorize]
    [EnableCors("CorsApi")]
    [Route("api/Floors")]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;
        public FloorController(FloorService floorService)
        {
            _floorService = floorService;
        }

        /// <summary>
        /// Gets all floors
        /// </summary>
        /// <returns>List of all floors</returns>
        /// <response code="200">List of all floors returned</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _floorService.GetFloors());
        }

        /// <summary>
        /// Fetches a specific floor
        /// </summary>
        /// <param name="id">Floor's id</param>
        /// <returns>The found floor, fail otherwise</returns>
        /// <response code="200">Found floor</response>
        /// <response code="200">User is not authorized to perform this action</response>
        /// <response code="404">Floor with specified id was not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _floorService.Get(id);
            return result == null ? NotFound($"Floor with id = {id} was not found.") : Ok(result);
        }

        /// <summary>
        /// Creates a new floor
        /// </summary>
        /// <param name="dormId">Dormitory id the floor belongs to</param>
        /// <param name="number">Floor's number (related to its elevation)</param>
        /// <returns>Success or error message</returns>
        /// <response code="201">Floor created</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Dormitory with specified id was not found</response>
        /// <response code="409">Floor with specified number already exists</response>
        [HttpPost]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Create(int dormId, int number)
        {
            var dorm = await _floorService.FindDorm(dormId);
            if (dorm == null)
                return NotFound($"Dormitory with id = {dormId} was not found.");

            return await _floorService.Create(dorm, number)
                ? CreatedAtAction("Create", $"Floor with {number} created.")
                : Conflict($"Floor with number = {number} already exists");
        }

        /// <summary>
        /// Updates a specific floor's properties
        /// </summary>
        /// <param name="id">Floor's id</param>
        /// <param name="number">Optional dorm number to set</param>
        /// <param name="dormId">Optional dorm id to set</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Floor was updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Floor with specified id was not found</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Update(int id, int? number, int? dormId)
        {
            if (number == null && dormId == null)
                return BadRequest();

            Dormitory dorm = null;
            if(dormId != null)
                dorm = await _floorService.FindDorm(dormId.Value);

            return await _floorService.Update(id, number, dorm) ? Ok() : NotFound($"Floor with id = {id} was not found.");
        }

        /// <summary>
        /// Deletes a specified floor
        /// </summary>
        /// <param name="id">Floor's id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Floor was deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Floor with specified id was not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _floorService.Remove(id) ? Ok() : NotFound($"Floor with id = {id} was not found.");
        }
    }
}
