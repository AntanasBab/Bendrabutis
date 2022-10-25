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

        [HttpGet("GetSpecificRooms")]
        public async Task<IActionResult> GetSpecificRooms(int dormId, int FloorId, int? RoomId)
        {
            var dorm = await _roomService.GetDorm(dormId);
            if (dorm == null) return NotFound("Dorm was not found");

            var rooms = await _roomService.GetSpecificRooms(dorm, FloorId);
            if(RoomId == null)
                return rooms == null ? NotFound($"Floor was not found") :
                    rooms.Count > 0 ? Ok(rooms.Select(x => new Room()
                    {
                        Area = x.Area,
                        Id = x.Id,
                        Number = x.Number,
                        NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                        Residents = x.Residents
                    })) : NotFound($"No rooms were found in this floor");
            else
            {
                return rooms == null ? NotFound($"Floor was not found") :
                    rooms.Count > 0 ? Ok(rooms.Select(x => new Room()
                    {
                        Area = x.Area,
                        Id = x.Id,
                        Number = x.Number,
                        NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                        Residents = x.Residents
                    }).Where(y => y.Id == RoomId.Value)) : NotFound($"No rooms were found in this floor");
            }
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

        [HttpGet("GetFreeRooms")]
        public async Task<IActionResult> GetFreeRooms(int dormId)
        {
            var dorm = await _roomService.GetDorm(dormId);
            if (dorm == null) return NotFound($"Dorm with specified id = {dormId} was not found");
            var roomlist = await _roomService.GetFreeRooms(dorm);
            return roomlist.Any(x => x != null)
                ? Ok(roomlist.Select(x => new Room()
                {
                    Area = x.Area,
                    Id = x.Id,
                    Number = x.Number,
                    NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                    Residents = x.Residents
                }))
                : NotFound("No empty rooms were found");
        }
    }
}
