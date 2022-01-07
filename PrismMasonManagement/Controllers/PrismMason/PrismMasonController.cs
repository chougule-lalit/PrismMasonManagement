using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize(Policy = "Permissions.Items.View")]
        public async Task<IActionResult> GetItemAsync()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }
    }
}