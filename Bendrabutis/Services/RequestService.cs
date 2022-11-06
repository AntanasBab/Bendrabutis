using System.Security.Claims;
using Bendrabutis.Auth;
using Bendrabutis.Entities;
using Bendrabutis.Models.Dtos;
using Bendrabutis.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bendrabutis.Services
{
    public class RequestService
    {
        private readonly DataContext _context;
        public readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public RequestService(DataContext context, UserManager<User> userManager,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public async Task<List<Request>> GetRequests()
        {
            return (await _context.Requests.Include(m => m.Author).ToListAsync()).OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task<Request?> Get(ClaimsPrincipal user, int id)
        {
            var req = await _context.Requests.FindAsync(id);

            if (user.IsInRole(DormitoryRoles.Admin) || user.IsInRole(DormitoryRoles.Owner))
            {
                return req;
            }

            var authResult = await _authorizationService.AuthorizeAsync(user, req, "RequestResourceOwner");
            return !authResult.Succeeded ? null : req;
        }

        public async Task<bool> Create(string authorId, NewPostDto dto)
        {
            var author = await _userManager.FindByIdAsync(authorId);
            if (author == null) return false;

            await _context.Requests.AddAsync(new Request()
            {
                Author = author,
                UserId = authorId,
                CreatedAt = DateTime.Now,
                Description = dto.Description,
                RequestType = dto.Type
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(ClaimsPrincipal user, int id, RequestType? type, string? description)
        {
            var req = await _context.Requests.FindAsync(id);
            if (req == null) return false;

            var authResult = await _authorizationService.AuthorizeAsync(user, req, "RequestResourceOwner");
            if (!authResult.Succeeded)
            {
                return false;
            }

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