using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Contracts.DTOs.Permission;
using PrismMasonManagement.Application.Administration.Seeding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrismMasonManagement.Application.Contracts.Administration.Interfaces;

namespace PrismMasonManagement.Api.Controllers.Administration
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : PrismMasonManagementBaseApiController
    {
        private readonly IUserRoleAppService _userRoleAppService;

        public UserRolesController(IUserRoleAppService userRoleAppService)
        {
            _userRoleAppService = userRoleAppService;
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRolesDto(string userId)
        {
            var model = await _userRoleAppService.GetUserRoleAsync(userId);
            return Ok(model);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(ManageUserRolesDto model)
        {
            var result = await _userRoleAppService.Update(model);
            return Ok(result);
        }
    }
}
