using Bendrabutis.Models;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [Route("api/Rooms")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roomService.GetRooms());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomService.GetRoom(id);
            return room == null ? Ok(room) : NotFound($"Room with id = {id} was not found");
        }

        [HttpPost]
        public async Task<IActionResult> Create(int floorId, int number, int numberOfLivingPlaces, double area)
        {
            var floor = await _roomService.GetFloor(floorId);
            if (floor == null) return NotFound($"Floor with specified id = {floorId} was not found");

            return await _roomService.Create(floor, number, numberOfLivingPlaces, area)
                ? CreatedAtAction("Create", $"Room with number {number} created.")
                : Conflict($"Room with number = {number} already exists in the specified floor");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, int? floorId, int? number, int? numberOfLivingPlaces,
            double? area)
        {
            if (floorId == null && number == null && numberOfLivingPlaces == null && area == null)
                return BadRequest("No properties to update were unspecified");

            return await _roomService.Update(id, floorId, number, numberOfLivingPlaces, area)
                ? Ok()
                : NotFound($"Room with specified id = {id} was not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _roomService.Delete(id) ? Ok() : NotFound($"Room with specified id = {id} was not found");
        }
    }
}
