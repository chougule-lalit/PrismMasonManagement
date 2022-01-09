using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs
{
    public interface IItemAppService : IPrismMasonManagementAppService
    {
        Task DeleteAsync(int id);

        Task<List<ItemDto>> GetAllItemsAync();
    }
}
