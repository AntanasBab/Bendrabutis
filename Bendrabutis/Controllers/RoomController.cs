using Bendrabutis.Auth;
using Bendrabutis.Entities;
using Bendrabutis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [EnableCors("CorsApi")]
    [Authorize(Roles = $"{DormitoryRoles.Owner}, {DormitoryRoles.Admin}")]
    [Route("api/Rooms")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Gets all rooms and their data
        /// </summary>
        /// <returns>List of all rooms</returns>
        /// <response code="200">List of all rooms</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roomService.GetRooms());
        }

        /// <summary>
        /// Gets a specific room
        /// </summary>
        /// <param name="id">Room's id</param>
        /// <returns>Found room or error message</returns>
        /// <response code="200">Found room</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Room with specified id was not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomService.GetRoom(id);
            return room == null ? Ok(room) : NotFound($"Room with id = {id} was not found");
        }

        /// <summary>
        /// Hierarchical room searched based on dormitory and, optionally, floor
        /// </summary>
        /// <param name="dormId">Dormitory id</param>
        /// <param name="FloorId">Optional floor id</param>
        /// <param name="RoomId">Optional room id</param>
        /// <returns>List of found rooms and their information</returns>
        /// <response code="200">Returned list of all rooms</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Dormitory, floor or room with specified id was not found.</response>
        [HttpGet("GetSpecificRooms")]
        public async Task<IActionResult> GetSpecificRooms(int dormId, int FloorId, int? RoomId)
        {
            var dorm = await _roomService.GetDorm(dormId);
            if (dorm == null) return NotFound("Dorm was not found");

            var rooms = await _roomService.GetSpecificRooms(dorm, FloorId);
            if (RoomId == null)
                return rooms == null ? NotFound($"Floor was not found") :
                    rooms.Count > 0 ? Ok(rooms.Select(x => new Room()
                    {
                        Area = x.Area,
                        Id = x.Id,
                        Number = x.Number,
                        NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                        Residents = x.Residents
                    })) : NotFound($"No rooms were found in this floor");

            return rooms == null ? NotFound($"Floor was not found") :
                rooms.Count > 0 ? Ok(rooms.Select(x => new Room()
                    {
                        Area = x.Area,
                        Id = x.Id,
                        Number = x.Number,
                        NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                        Residents = x.Residents
                    })
                    .Where(y => y.Id == RoomId.Value)) : NotFound($"No rooms were found in this floor");
        }

        /// <summary>
        /// Creates a new room
        /// </summary>
        /// <param name="floorId">Floor id</param>
        /// <param name="number">Room number</param>
        /// <param name="numberOfLivingPlaces">How many living places are in a room</param>
        /// <param name="area">Room's area</param>
        /// <returns>Success or error message</returns>
        /// <response code="201">Room created successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Floor with specified id was not found</response>
        /// <response code="409">Room with specified number already exists</response>
        [HttpPost]
        public async Task<IActionResult> Create(int floorId, int number, int numberOfLivingPlaces, double area)
        {
            var floor = await _roomService.GetFloor(floorId);
            if (floor == null) return NotFound($"Floor with specified id = {floorId} was not found");

            return await _roomService.Create(floor, number, numberOfLivingPlaces, area)
                ? CreatedAtAction("Create", $"Room with number {number} created.")
                : Conflict($"Room with number = {number} already exists in the specified floor");
        }

        /// <summary>
        /// Updates a specified room
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="floorId">Optional floor id to set</param>
        /// <param name="number">Optional room number to set</param>
        /// <param name="numberOfLivingPlaces">Optional room's number of living places to set</param>
        /// <param name="area">Optional room area to ser</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Room updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">No parameters were used in API call</response>
        /// <response code="404">Room with specified id was not found</response>
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

        /// <summary>
        /// Deletes a specific room
        /// </summary>
        /// <param name="id">Room id to delete</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Room deleted successfully</response>
        /// <response code="404">Room with specified id was not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _roomService.Delete(id) ? Ok() : NotFound($"Room with specified id = {id} was not found");
        }

        /// <summary>
        /// Gets all of the dormitories' rooms which have an an available living place
        /// </summary>
        /// <param name="dormId">Dormitory id</param>
        /// <returns>List of found not completely occupied rooms</returns>
        /// <response code="200">Returned list of found rooms</response>
        /// <response code="404">Dormitory with specified id was not found or no rooms were available</response>
        [HttpGet("GetFreeRooms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFreeRooms(int dormId)
        {
            var dorm = await _roomService.GetDorm(dormId);
            if (dorm == null) return NotFound($"Dorm with specified id = {dormId} was not found");
            var roomlist = await _roomService.GetFreeRooms(dorm);
            return roomlist.Any(x => x != null)
                ? Ok(roomlist.Select(x => new
                {
                    Area = x.Area,
                    Id = x.Id,
                    Number = x.Number,
                    NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                }))
                : NotFound("No empty rooms were found");
        }

        /// <summary>
        /// Assign a user as resident in a specific room
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="userId">User id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User assigned successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">Bad request parameters. See the error message for more details.</response>
        [HttpPost("AssignRoom/{id}")]
        public async Task<IActionResult> AssignRoom(int id, string userId)
        {
            var result = await _roomService.AssignRoom(id, userId);
            return result == string.Empty ? Ok("User has been added to the room.") : BadRequest(result);
        }

        /// <summary>
        /// Remove a user as resident in a specific room
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="userId">User id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User assigned successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">Bad request parameters. See the error message for more details.</response>
        [HttpPost("RemoveFromRoom/{id}")]
        public async Task<IActionResult> RemoveFromRoom(int id, string userId)
        {
            return await _roomService.RemoveFromRoom(id, userId) ? Ok("User has been removed from the room.") : BadRequest("Failed to find specified user or room.");
        }
    }
}