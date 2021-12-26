using Microsoft.AspNetCore.Identity;
using PrismMasonManagement.Api.Constants;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.Seeding
{
    public static class PrismMasonRoleSeeder
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
