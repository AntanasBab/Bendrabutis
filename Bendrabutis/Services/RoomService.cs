using Bendrabutis.Entities;
using Bendrabutis.Models.DbModels;
using Microsoft.AspNetCore.Identity;

namespace Bendrabutis.Services
{
    public class RoomService
    {
        private readonly DataContext _context;
        public readonly UserManager<User> _userManager;

        public RoomService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<RoomReadModel>> GetRooms()
        {
            return await _context.Rooms.Include(m => m.Residents)
                .Select(x => new RoomReadModel
                {
                    Area = x.Area,
                    Number = x.Number,
                    Id = x.Id,
                    NumberOfLivingPlaces = x.NumberOfLivingPlaces,
                    Residents = x.Residents.Select(y => new UserReadModel {Id = y.Id}).ToList()
                })
                .ToListAsync();
        }

        public async Task<Room?> GetRoom(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Floor?> GetFloor(int id) => await _context.Floors.FindAsync(id);

        public async Task<Dormitory?> GetDorm(int id) =>
            (await _context.Dormitories.Include(m => m.Floors).ToListAsync()).FirstOrDefault(x => x.Id == id);

        public async Task<List<Room>> GetSpecificRooms(Dormitory dorm, int floorId)
        {
            var floor = dorm.Floors.FirstOrDefault(x => x.Id == floorId);
            if (floor == null) return null;

            return (await _context.Rooms.ToListAsync()).Where(x => x.Floor.Id == floorId).ToList();
        }

        public async Task<bool> Create(Floor floor, int number, int numberOfLivingPlaces, double area)
        {
            if (await _context.Rooms.AnyAsync(x => x.Number == number && x.Floor.Id == floor.Id)) return false;

            await _context.Rooms.AddAsync(new Room()
            {
                Floor = floor, Number = number, NumberOfLivingPlaces = numberOfLivingPlaces, Area = area
            });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(int id, int? floorId, int? number, int? numberOfLivingPlaces, double? area)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            Floor? floor = null;
            if (floorId != null) floor = await GetFloor(floorId.Value);

            if (floor != null) room.Floor = floor;
            if (number != null) room.Number = number.Value;
            if (numberOfLivingPlaces != null) room.NumberOfLivingPlaces = numberOfLivingPlaces.Value;
            if (area != null) room.Area = area.Value;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Room>> GetFreeRooms(Dormitory dorm)
        {
            List<Room> result = new List<Room>();
            foreach (var floor in dorm.Floors)
            {
                var rooms = (await _context.Rooms.ToListAsync()).Where(x =>
                    x.Floor.Id == floor.Id && x.NumberOfLivingPlaces > x.Residents.Count());
                result.AddRange(rooms);
            }

            return result;
        }

        public async Task<string> AssignRoom(int roomId, string userId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return $"Room with specified id = {roomId} was not found.";

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return $"User with specified id = {userId} was not found.";

            if (room.Residents.Count() >= room.NumberOfLivingPlaces)
                return "The specified room is already fully occupied";

            room.Residents.Add(user);
            await _context.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<bool> RemoveFromRoom(int roomId, string userId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            room.Residents.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}