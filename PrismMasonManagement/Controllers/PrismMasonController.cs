using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Data;
using PrismMasonManagement.Models;

namespace PrismMasonManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrismMasonController : ControllerBase
    {
        private readonly PrismMasonDbContext _context;
        public PrismMasonController(PrismMasonDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("getItems")]
        public async Task<IActionResult> GetItemAsync()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }
    }
}