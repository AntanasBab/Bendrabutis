using Bendrabutis.Models;

namespace Bendrabutis.Services
{
    public class FloorService
    {
        private readonly DataContext _context;
        public FloorService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Floor>> GetFloors()
        {
            return await _context.Floors.ToListAsync();
        }

        public async Task<Floor?> Get(int id)
        {
            return await _context.Floors.FindAsync(id);
        }

        public async Task<Dormitory?> FindDorm(int dormId)
        {
            return await _context.Dormitories.FindAsync(dormId);
        }

        public async Task<bool> Create(Dormitory dormitory, int number)
        {
            await _context.Floors.AddAsync(new Floor() { Number = number, Dormitory = dormitory });
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Remove(int id)
        {
            var floor = await _context.Floors.FindAsync(id);
            if (floor == null)
                return false;

            _context.Floors.Remove(floor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(int id, int? number, Dormitory? dorm)
        {
            var floor = await _context.Floors.FindAsync(id);
            if (floor == null)
                return false;

            if(number != null)
                floor.Number = number.Value;

            if (dorm != null)
                floor.Dormitory = dorm;

            _context.Floors.Update(floor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
