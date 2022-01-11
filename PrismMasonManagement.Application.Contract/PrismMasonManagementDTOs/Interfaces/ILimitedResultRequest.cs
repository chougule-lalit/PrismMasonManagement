using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.Interfaces
{
    public interface ILimitedResultRequest
    {
        //
        // Summary:
        //     Maximum result count should be returned. This is generally used to limit result
        //     count on paging.
        int MaxResultCount { get; set; }
    }
}
