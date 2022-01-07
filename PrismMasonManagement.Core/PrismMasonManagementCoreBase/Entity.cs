using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Core.PrismMasonManagementCoreBase
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }
    }
}
