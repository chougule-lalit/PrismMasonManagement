using Microsoft.AspNetCore.Identity;
using PrismMasonManagement.Core.PrismMasonManagementCoreBase;
using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Core
{
    public class AppUser : IdentityUser, ISoftDelete, IFullAuditedEntity
    {
        public virtual Guid? DeletorId { get; set; }
        public virtual DateTime? DeletedDateTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual Guid? CreatorId { get; set; }
    }
}
