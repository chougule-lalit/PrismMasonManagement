using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface
{
    public interface ISoftDelete
    {
        Guid? DeletorId { get; set; }
        DateTime? DeletedDateTime { get; set; }
        bool IsDeleted { get; set; }
    }
}
