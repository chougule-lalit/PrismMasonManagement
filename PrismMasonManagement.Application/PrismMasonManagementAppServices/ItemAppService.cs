using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using PrismMasonManagement.Core.Entities;
using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
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
        private readonly IUnitOfWork _unitOfWork;

        public ItemAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _unitOfWork.Repository<Item>().Delete(id);
            var result = await _unitOfWork.Complete();
            if (result > 0)
                return true;
            else 
                return false;
        }

        public async Task<List<ItemDto>> GetAllItemsAync()
        {
            var query = await _unitOfWork.Repository<Item>().GetAll().ToListAsync();
            await _unitOfWork.Complete();
            return ObjectMapper.Map<List<Item>, List<ItemDto>>(query);
        }
    }
}
