using PrismMasonManagement.Application.Contracts.Administration.DTOs;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.Administration.Interfaces
{
    public interface IUserAppService : IPrismMasonManagementAppService
    {
        Task<PagedResultDto<GetUserOutputDto>> GetPagedResultAsync(GetUserInputDto input);
        Task<bool> CreateAsync(UserDto input);
        Task<bool> UpdateAsync(Guid id, UserDto input);
        Task<UserDto> GetAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);

    }
}
