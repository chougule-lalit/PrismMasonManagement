using Microsoft.AspNetCore.Identity;
using PrismMasonManagement.Application.Contracts.DTOs.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Authorization
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsDto> allPermissions, List<Type> policies, string roleId)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
                foreach (var policy in policies)
            {
                fields.AddRange(policy.GetFields(BindingFlags.Static | BindingFlags.Public));
            }
                
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsDto { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
            }
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
