using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Route("api/Rooms")]
    public class RoomController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string address, int? roomCapacity)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, string? name = null, string? address = null, int? roomCapacity = null)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
