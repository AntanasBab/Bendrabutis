using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bendrabutis.Auth;
using Bendrabutis.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bendrabutis.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        public readonly UserManager<User> _userManager;

        public UserService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Update(ClaimsPrincipal reqUser, string id, string? username, string? email,
            string? role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            if (await _userManager.IsInRoleAsync(user, DormitoryRoles.Owner)) return false;

            if (reqUser.IsInRole(DormitoryRoles.Admin) || reqUser.IsInRole(DormitoryRoles.Owner))
            {
                if (role != null && DormitoryRoles.All.Any(x => x == role))
                {
                    var currRole = await _userManager.GetRolesAsync(user);
                    var result = await _userManager.RemoveFromRolesAsync(user, currRole);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }

                if (username != null)
                {
                    user.UserName = username;
                }

                if (email != null)
                {
                    user.Email = email;
                }

                await _userManager.UpdateAsync(user);
            }
            else
            {
                if (reqUser.FindFirstValue(JwtRegisteredClaimNames.Sub) != id)
                {
                    return false;
                }

                if (username != null)
                {
                    user.UserName = username;
                }

                if (email != null)
                {
                    user.Email = email;
                }

                await _userManager.UpdateAsync(user);
            }

            return true;
        }

        public async Task<bool> Delete(ClaimsPrincipal reqUser, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            if (await _userManager.IsInRoleAsync(user, DormitoryRoles.Owner)) return false;

            if (await _userManager.IsInRoleAsync(
                    await _userManager.FindByIdAsync(reqUser.FindFirstValue(JwtRegisteredClaimNames.Sub)),
                    DormitoryRoles.Owner))
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            else
            {
                if (await _userManager.IsInRoleAsync(user, DormitoryRoles.Admin)) return false;
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
        }

        public async Task<bool> ChangePassword(string id, string oldPass, string newPass)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, oldPass, newPass);
            return result.Succeeded;
        }
    }
}