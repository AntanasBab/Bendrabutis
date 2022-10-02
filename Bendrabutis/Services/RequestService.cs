using Bendrabutis.Models;
using Bendrabutis.Models.Enums;

namespace Bendrabutis.Services
{
    public class RequestService
    {
        private readonly DataContext _context;
        public RequestService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Request>> GetRequests()
        {
            return (await _context.Requests.ToListAsync()).OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task<Request?> Get(int id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async Task<bool> Create(User author, RequestType type, string description)
        {
            await _context.Requests.AddAsync(new Request()
            {
                Author = author, CreatedAt = DateTime.Now, Description = description, RequestType = type
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(int id, User? author, RequestType? type, string? description)
        {
            var req = await _context.Requests.FindAsync(id);
            if(req == null) return false;

            if(author != null) req.Author = author;
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
