using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.Administration.DTOs
{
    public class GetUserInputDto : PagedResultRequestDto
    {
        public List<string> RoleNames { get; set; }
    }
}
