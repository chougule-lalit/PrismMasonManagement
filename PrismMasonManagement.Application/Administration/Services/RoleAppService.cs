using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrismMasonManagement.Application.Contracts.Administration.DTOs;
using PrismMasonManagement.Application.Contracts.Administration.Interfaces;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Administration.Services
{
    public class RoleAppService : PrismMasonManagementAppService, IRoleAppService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleAppService> _logger;

        public RoleAppService(
            RoleManager<IdentityRole> roleManager,
            ILogger<RoleAppService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public virtual async Task<PagedResultDto<RoleDto>> PagedResultAsync(GetRoleInputDto input)
        {
            var roles = await _roleManager.Roles.Select(x => new RoleDto
            {
                Id = x.Id != null ? Guid.Parse(x.Id) : Guid.Empty,
                Name = x.Name
            }).ToListAsync();

            var count = roles.Count;
            var finalRolesList = roles.Skip(input.SkipCount * input.MaxResultCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<RoleDto>
            {
                Items = finalRolesList,
                TotalCount = count
            };
        }

        public virtual async Task<bool> CreateAsync(string roleName)
        {
            if (roleName != null)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                return result.Succeeded;
            }

            return false;
        }

        public virtual async Task<bool> UpdateAsync(RoleDto input)
        {
            if (input != null)
            {
                var role = await _roleManager.FindByIdAsync(input.Id.ToString());
                if (role != null)
                {

                    role.Name = input.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    return result.Succeeded;
                }
                else
                {
                    _logger.LogError($"Role not found with Id : {input.Id}");
                    return false;
                }
            }
            else
            {
                _logger.LogError($"Input cannot be empty");
                return false;
            }
            
        }

        public virtual async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                return ObjectMapper.Map<IdentityRole, RoleDto>(role);
            }
            else
            {
                _logger.LogError($"Role not found with Id : {id}");
                return new RoleDto();
            }
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            else
            {
                _logger.LogError($"Role not found with Id : {id}");
                return false;
            }
        }
    }
}
