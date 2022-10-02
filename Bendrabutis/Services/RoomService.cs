using Bendrabutis.Models;

namespace Bendrabutis.Services
{
    public class RoomService
    {
        private readonly DataContext _context;

        public RoomService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room?> GetRoom(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Floor?> GetFloor(int id) => await _context.Floors.FindAsync(id);

        public async Task<bool> Create(Floor floor, int number, int numberOfLivingPlaces, double area)
        {
            if (await _context.Rooms.AnyAsync(x => x.Number == number)) return false;

            await _context.Rooms.AddAsync(new Room()
            {
                Floor = floor, Number = number, NumberOfLivingPlaces = numberOfLivingPlaces, Area = area
            });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(int id, Floor? floor, int? number, int? numberOfLivingPlaces, double? area)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

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
    }
}
