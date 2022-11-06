using Bendrabutis.Entities;

namespace Bendrabutis.Services
{
    public class UserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
