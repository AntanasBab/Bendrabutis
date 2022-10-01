using Bendrabutis.Models;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            return Ok(JsonConvert.SerializeObject(await _dormitoryService.GetAllDormitories()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dormitory = await _dormitoryService.GetDormitory(id);
            return dormitory != null ? Ok(JsonConvert.SerializeObject(dormitory)) : NotFound($"Dormitory with id = {id} was not found.");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string address, int roomCapacity)
        {
            return await _dormitoryService.CreateDormitory(name, address, roomCapacity)
                ? CreatedAtAction("Create", name)
                : Conflict("Dormitory with specified name already exists.");
        }
    }
}
