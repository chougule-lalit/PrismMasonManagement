using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO
{
    public class FullAuditedEntityDto
    {
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
