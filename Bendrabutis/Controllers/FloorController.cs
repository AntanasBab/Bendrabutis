using Bendrabutis.Models;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Route("api/Floors")]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;
        public FloorController(FloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _floorService.GetFloors());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _floorService.Get(id);
            return result == null ? NotFound($"Floor with id = {id} was not found.") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int dormId, int number)
        {
            var dorm = await _floorService.FindDorm(dormId);
            if (dorm == null)
                return NotFound($"Dormitory with id = {dormId} was not found.");

            return await _floorService.Create(dorm, number)
                ? CreatedAtAction("Create", $"Floor with {number} created.")
                : Conflict($"Floor with number = {number} already exists");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, int? number, int? dormId)
        {
            if (number == null && dormId == null)
                return BadRequest();

            Dormitory dorm = null;
            if(dormId != null)
                dorm = await _floorService.FindDorm(dormId.Value);

            return await _floorService.Update(id, number, dorm) ? Ok() : NotFound($"Floor with id = {id} was not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _floorService.Remove(id) ? Ok() : NotFound($"Floor with id = {id} was not found.");
        }
    }
}
