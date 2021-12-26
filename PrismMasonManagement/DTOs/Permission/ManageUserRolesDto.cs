using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.DTOs.Permission
{
    public class ManageUserRolesDto
    {
        public string UserId { get; set; }
        public IList<UserRolesDto> UserRoles { get; set; }
    }
}
