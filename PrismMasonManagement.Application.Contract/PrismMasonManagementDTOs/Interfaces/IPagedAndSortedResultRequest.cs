using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.Interfaces
{
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ISortedResultRequest
    {
    }
}
