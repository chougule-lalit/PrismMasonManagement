using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Authorization;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs;
using PrismMasonManagement.Core.Entities;
using PrismMasonManagement.Infrastructure;

namespace PrismMasonManagement.Api.Controllers.PrismMason
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrismMasonController : ControllerBase
    {
        private readonly PrismMasonManagementDbContext _context;
        private readonly IItemAppService _itemAppService;

        public PrismMasonController(PrismMasonManagementDbContext context,
            IItemAppService itemAppService)
        {
            _context = context;
            _itemAppService = itemAppService;

        }

        [HttpGet]
        [Route("getItems")]
        [Authorize(Policy = Permissions.Items.View)]
        public async Task<IActionResult> GetItemAsync()
        {
            var items = await _itemAppService.GetAllItemsAync();
            return Ok(items);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = Permissions.Items.Create)]
        public async Task<IActionResult> CreateAsync(string name)
        {
            var data = new Item
            {
                Name = name
            };

            await _context.Items.AddAsync(data);
            var ddd = await _context.SaveChangesAsync();

            return Ok(ddd);
        }

        [HttpPost]
        [Route("update")]
        [Authorize(Policy = Permissions.Items.Edit)]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await _context.Items
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                result.Name = "Some Test";

                await _context.SaveChangesAsync();

                return Ok(true);
            }

            return Ok(false);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Policy = Permissions.Items.Delete)]
        public async Task DeleteAsync(int id)
        {
            await _itemAppService.DeleteAsync(id);
        }
    }
}