using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using PrismMasonManagement.Core.Entities;
using PrismMasonManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.PrismMasonManagementAppServices
{
    public class ItemAppService : PrismMasonManagementAppService, IItemAppService
    {
        private readonly PrismMasonManagementDbContext _context;

        public ItemAppService(
            PrismMasonManagementDbContext context) 
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Items
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Items.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ItemDto>> GetAllItemsAync()
        {
            var result = await _context.Items.ToListAsync();
            var returnData = ObjectMapper.Map<List<Item>, List<ItemDto>>(result);
            return returnData;
        }
    }
}
