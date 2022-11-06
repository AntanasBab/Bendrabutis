using Bendrabutis.Auth;
using Bendrabutis.Entities;
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
            await AddUsers();
        }

        private async Task AddUsers()
        {
            var ownerUser = new User()
            {
                UserName = "owner",
                Email = "owner@ktu.lt",
            };
            var adminUser = new User()
            {
                UserName = "admin",
                Email = "admin@ktu.lt",
            };
            var visitorUser = new User()
            {
                UserName = "visitor",
                Email = "visitor@ktu.lt",
            };
            var residentUser = new User()
            {
                UserName = "resident",
                Email = "resident@ktu.lt",
            };

            // Owner.
            var existingUser = await _userManager.FindByEmailAsync(ownerUser.Email);
            if (existingUser == null)
            {
                var createdOwnerUser = await _userManager.CreateAsync(ownerUser, "Owner123");
                if (createdOwnerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(ownerUser, DormitoryRoles.Owner);
                }
            }

            // Admin.
            existingUser = await _userManager.FindByEmailAsync(adminUser.Email);
            if (existingUser == null)
            {
                var createdAdminUser = await _userManager.CreateAsync(adminUser, "Admin123");
                if (createdAdminUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, DormitoryRoles.Admin);
                }
            }

            // Visitor.
            existingUser = await _userManager.FindByEmailAsync(visitorUser.Email);
            if (existingUser == null)
            {
                var createdVisitorUser = await _userManager.CreateAsync(visitorUser, "Visitor123");
                if (createdVisitorUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(visitorUser, DormitoryRoles.Visitor);
                }
            }

            // Resident.
            existingUser = await _userManager.FindByEmailAsync(residentUser.Email);
            if (existingUser == null)
            {
                var createdResidentUser = await _userManager.CreateAsync(residentUser, "Resident123");
                if (createdResidentUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(residentUser, DormitoryRoles.Resident);
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
