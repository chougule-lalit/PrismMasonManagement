using Microsoft.AspNetCore.Identity;
using PrismMasonManagement.Core;
using PrismMasonManagement.Core.Constants;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Administration.Seeding
{
    public static class PrismMasonRoleSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
