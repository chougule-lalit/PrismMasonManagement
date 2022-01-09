using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService
{
    public interface ICurrentUser
    {
        Guid Id { get; }
    }
}
