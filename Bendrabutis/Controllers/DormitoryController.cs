
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Route("api/Dormitories")]
    public class DormitoryController : ControllerBase
    {
        private readonly DormitoryService _dormitoryService;

        public DormitoryController(DormitoryService dormitoryService)
        {
            _dormitoryService = dormitoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dormitoryService.GetAllDormitories());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dormitory = await _dormitoryService.GetDormitory(id);
            return dormitory != null ? Ok(dormitory) : NotFound($"Dormitory with id = {id} was not found.");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string address, int? roomCapacity)
        {
            if (roomCapacity == null)
                return BadRequest("Room capacity is required.");

            return await _dormitoryService.CreateDormitory(name, address, roomCapacity.Value)
                ? CreatedAtAction("Create", $"Dormitory with {name} created.")
                : Conflict("Dormitory with specified name already exists.");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, string? name = null, string? address = null, int? roomCapacity = null)
        {
            if(name == null && address == null && roomCapacity == null)
                return BadRequest();

            return await _dormitoryService.UpdateDormitory(id, name, address, roomCapacity) ? Ok() : NotFound($"Dormitory with id = {id} was not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _dormitoryService.DeleteDormitory(id) ? Ok($"Dormitory with id = {id} removed.") : NotFound($"Dormitory with id = {id} was not found.");
        }
    }
}
