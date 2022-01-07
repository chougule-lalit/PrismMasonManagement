using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO
{
    public class ItemDto : FullAuditedEntityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
