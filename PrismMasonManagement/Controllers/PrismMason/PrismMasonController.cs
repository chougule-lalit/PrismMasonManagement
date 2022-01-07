using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Authorization;
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
        public PrismMasonController(PrismMasonManagementDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("getItems")]
        [Authorize(Policy = Permissions.Items.View)]
        public async Task<IActionResult> GetItemAsync()
        {
            var items = await _context.Items.ToListAsync();
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
    }
}