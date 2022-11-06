using Bendrabutis.Auth;
using Bendrabutis.Models;
using Bendrabutis.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bendrabutis.Data
{
    public class DbSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddOwnerUser();
            await AddAdminUser();
        }

        private async Task AddOwnerUser()
        {
            var ownerUser = new User()
            {
                UserName = "owner",
                Email = "owner@ktu.lt",
            };

            var existingUser = await _userManager.FindByEmailAsync(ownerUser.Email);
            if (existingUser == null)
            {
                var createdAdminUser = await _userManager.CreateAsync(ownerUser, "Owner123");
                if (createdAdminUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(ownerUser, DormitoryRoles.Owner);
                }
            }
        }

        private async Task AddAdminUser()
        {
            var adminUser = new User()
            {
                UserName = "admin",
                Email = "admin@ktu.lt",
            };

            var existingUser = await _userManager.FindByEmailAsync(adminUser.Email);
            if (existingUser == null)
            {
                var createdAdminUser = await _userManager.CreateAsync(adminUser, "Admin123");
                if (createdAdminUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, DormitoryRoles.Admin);
                }
            }
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in DormitoryRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
