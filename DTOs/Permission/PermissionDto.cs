using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.DTOs.Permission
{
    public class PermissionDto
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsDto> RoleClaims { get; set; }
    }
}
