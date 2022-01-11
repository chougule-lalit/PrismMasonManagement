using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismMasonManagement.Application.Authorization;
using PrismMasonManagement.Application.Contracts.Administration.DTOs;
using PrismMasonManagement.Application.Contracts.Administration.Interfaces;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.Controllers.Administration
{
    [Authorize]
    public class UserController : PrismMasonManagementBaseApiController, IUserAppService
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost]
        [Route("getAllUsers")]
        [Authorize(Policy = Permissions.Users.View)]
        public virtual async Task<PagedResultDto<GetUserOutputDto>> GetPagedResultAsync(GetUserInputDto input)
        {
            return await _userAppService.GetPagedResultAsync(input);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = Permissions.Users.Create)]
        public virtual async Task<bool> CreateAsync([FromBody] UserDto input)
        {
            return await _userAppService.CreateAsync(input);
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize(Policy = Permissions.Users.View)]
        public virtual async Task<UserDto> GetAsync(Guid id)
        {
            return await _userAppService.GetAsync(id);
        }

        [HttpPost]
        [Route("update/{id}")]
        [Authorize(Policy = Permissions.Users.Edit)]
        public virtual async Task<bool> UpdateAsync(Guid id, [FromBody] UserDto input)
        {
            return await _userAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Policy = Permissions.Users.Delete)]
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            return await _userAppService.DeleteAsync(id);
        }
    }
}
