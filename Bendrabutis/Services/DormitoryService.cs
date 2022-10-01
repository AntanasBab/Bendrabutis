using Bendrabutis.Models;

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
            return await _dataContext.Dormitories.ToListAsync();
        }

        public async Task<Dormitory?> GetDormitory(int id)
        {
            return await _dataContext.Dormitories.FindAsync(id);
        }

        public async Task<bool> CreateDormitory(string name, string address, int roomCapacity)
        {
            if (await _dataContext.Dormitories.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower())))
                return false;

            var success = _dataContext.Dormitories.AddAsync(new Dormitory() { Name = name, Address = address, RoomCapacity = roomCapacity }).IsCompletedSuccessfully;
            await _dataContext.SaveChangesAsync();
            return success;
        }
    }
}
