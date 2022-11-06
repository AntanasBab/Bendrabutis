using Bendrabutis.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Services
{
    public class DormitoryService
    {
        private readonly DataContext _dataContext;

        public DormitoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Dormitory>> GetAllDormitories()
        {
            return await _dataContext.Dormitories.Include(m => m.Floors).ToListAsync();
        }

        public async Task<Dormitory?> GetDormitory(int id)
        {
            return _dataContext.Dormitories.Include(m => m.Floors).ToList().FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> CreateDormitory(string name, string address, int roomCapacity)
        {
            if (await _dataContext.Dormitories.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()))) return false;

            var success = _dataContext.Dormitories
                .AddAsync(new Dormitory() {Name = name, Address = address, RoomCapacity = roomCapacity})
                .IsCompletedSuccessfully;
            await _dataContext.SaveChangesAsync();
            return success;
        }

        public async Task<bool> UpdateDormitory(int id, string? name = null, string? address = null,
            int? roomCapacity = null)
        {
            var dorm = await _dataContext.Dormitories.FindAsync(id);
            if (dorm == null) return false;

            if (name != null) dorm.Name = name;

            if (address != null) dorm.Address = address;

            if (roomCapacity != null) dorm.RoomCapacity = roomCapacity.Value;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDormitory(int id)
        {
            var dorm = await _dataContext.Dormitories.FindAsync(id);
            if (dorm == null) return false;

            _dataContext.Dormitories.Remove(dorm);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
