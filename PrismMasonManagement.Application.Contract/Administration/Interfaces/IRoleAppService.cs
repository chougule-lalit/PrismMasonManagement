using PrismMasonManagement.Application.Contracts.Administration.DTOs;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.Administration.Interfaces
{
    public interface IRoleAppService : IPrismMasonManagementAppService
    {
        Task<PagedResultDto<RoleDto>> PagedResultAsync(GetRoleInputDto input);
        Task<bool> CreateAsync(string roleName);
        Task<bool> UpdateAsync(RoleDto input);
        Task<RoleDto> GetAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
