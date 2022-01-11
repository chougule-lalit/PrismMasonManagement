using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO
{
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        public PagedResultRequestDto() { }

        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}
