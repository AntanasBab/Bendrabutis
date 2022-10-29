using Bendrabutis.Models;
using Bendrabutis.Models.Dtos;
using Bendrabutis.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bendrabutis.Services
{
    public class RequestService
    {
        private readonly DataContext _context;
        public readonly UserManager<User> _userManager;
        public RequestService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Request>> GetRequests()
        {
            return (await _context.Requests.Include(m => m.Author).ToListAsync()).OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task<Request?> Get(int id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async Task<User?> GetUser(int userId) => await _context.Users.FindAsync(userId);

        public async Task<bool> Create(string authorId, NewPostDto dto)
        {
            if (authorId == null) return false;

            var author = await _userManager.FindByIdAsync(authorId);
            if (author == null) return false;

            await _context.Requests.AddAsync(new Request()
            {
                Author = author, CreatedAt = DateTime.Now, Description = dto.Description, RequestType = dto.Type
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(int id, RequestType? type, string? description)
        {
            var req = await _context.Requests.FindAsync(id);
            if(req == null) return false;

            if (type != null) req.RequestType = type.Value;
            if (description != null) req.Description = description;

            _context.Update(req);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            var req = await _context.Requests.FindAsync(id);
            if (req == null) return false;

            _context.Requests.Remove(req);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
