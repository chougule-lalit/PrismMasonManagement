using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Contracts.Authorization.Interfaces;
using PrismMasonManagement.Application.Contracts.DTOs.Permission;
using PrismMasonManagement.Application.Seeding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.Controllers.Administration
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : ControllerBase
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
