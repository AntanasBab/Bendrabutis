using Bendrabutis.Models;
using Bendrabutis.Models.Enums;
using System.Security.Cryptography;

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

        public async Task<bool> Create(string username, string password, string fullName, string phoneNumber)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username)) return false;

            await _context.Users.AddAsync(new User
            {
                Username = username,
                PasswordHash = password,
                Role = UserRoles.Visitor,
                FullName = fullName,
                PhoneNumber = phoneNumber
            });
            return true;
        }

        public async Task<bool> AssignRoom(int id, Room room)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role == UserRoles.Administrator) return false;

            user.Room = room;
            if (user.Role == UserRoles.Visitor) user.Role = UserRoles.Resident;

            return true;
        }

        public async Task<bool> RemoveFromRoom(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role == UserRoles.Administrator) return false;

            user.Room = null;
            user.Role = UserRoles.Visitor;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(int id, string? username, string? password, string? fullName, string? phoneNumber)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            if (username != null) user.Username = username;
            if (password != null) user.PasswordHash = password;
            if (fullName != null) user.FullName = fullName;
            if (phoneNumber != null) user.PhoneNumber = phoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return true;
        }
    }
}
