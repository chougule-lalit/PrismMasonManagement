using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrismMasonManagement.Application.Contracts.Administration.DTOs;
using PrismMasonManagement.Application.Contracts.Administration.Interfaces;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using PrismMasonManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Administration.Services
{
    public class UserAppService : PrismMasonManagementAppService, IUserAppService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserAppService> _logger;

        public UserAppService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserAppService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public virtual async Task<PagedResultDto<GetUserOutputDto>> GetPagedResultAsync(GetUserInputDto input)
        {
            var usersList = new List<AppUser>();
            if (input?.RoleNames.Count > 0)
            {
                foreach (var role in input.RoleNames)
                {
                    usersList.AddRange(await _userManager.GetUsersInRoleAsync(role));
                }
            }
            else
            {
                usersList = await _userManager.Users.ToListAsync();
            }

            var count = usersList.Count;
            var finalUsers = usersList.Skip(input.SkipCount * input.MaxResultCount).Take(input.MaxResultCount).ToList();

            var outputList = new List<GetUserOutputDto>();
            foreach (var user in finalUsers)
            {
                var outputData = ObjectMapper.Map<AppUser, GetUserOutputDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                outputData.RoleList = roles.ToList();
                outputData.Roles = roles.Count > 0 ? string.Join(',', roles) : string.Empty;

                outputList.Add(outputData);
            }

            return new PagedResultDto<GetUserOutputDto>
            {
                Items = outputList,
                TotalCount = count
            };
        }

        public virtual async Task<bool> CreateAsync(UserDto input)
        {
            var user = await _userManager.Users.Where(x => x.PhoneNumber == input.PhoneNumber || x.Email == input.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                var newUser = new AppUser()
                {
                    Email = input.Email,
                    UserName = input.UserName,
                    PhoneNumber = input.PhoneNumber
                };

                var isCreated = await _userManager.CreateAsync(newUser, input.Password);

                foreach (var role in input.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                        await _userManager.AddToRoleAsync(newUser, role);
                }

                if (!isCreated.Succeeded)
                {
                    _logger.LogError($"Error occured while user creation");
                    return false;
                }
                else
                    return true;
            }
            else
            {
                _logger.LogError($"User already exist!!!");
                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(Guid id, UserDto input)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.UserName = input.UserName;
                user.Email = input.Email;
                user.PhoneNumber = input.PhoneNumber;

                var rolesToRemove = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                foreach (var role in input.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                        await _userManager.AddToRoleAsync(user, role);
                }

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError($"Error occured while updating user with id : {id}");
                    return false;
                }
                else
                    return true;

            }
            else
            {
                _logger.LogError($"User not found with Id : {id}");
                return false;
            }
        }

        public virtual async Task<UserDto> GetAsync(Guid id)
        {
            var userDto = new UserDto();
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                userDto = ObjectMapper.Map<AppUser, UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();
            }
            else
            {
                _logger.LogError($"User not found with id : {id}");
            }

            return userDto;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            else
            {
                _logger.LogError($"User not found with id : {id}");
                return false;
            }
        }
    }
}
