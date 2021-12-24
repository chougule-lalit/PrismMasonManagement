﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.DTOs.Permission;
using PrismMasonManagement.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRolesDto(string userId)
        {
            var viewModel = new List<UserRolesDto>();
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var roleName in roles.Select(x => x.Name))
            {
                var userRolesViewModel = new UserRolesDto
                {
                    RoleName = roleName
                };
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesDto()
            {
                UserId = userId,
                UserRoles = viewModel
            };
            return Ok(model);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(ManageUserRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            //Exception handling should be implemented here
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
                await _signInManager.RefreshSignInAsync(currentUser);
            await PrismMasonUserSeeder.SeedSuperAdminAsync(_userManager, _roleManager);
            return Ok(true);
        }
    }
}