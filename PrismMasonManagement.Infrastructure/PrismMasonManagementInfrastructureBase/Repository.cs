using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Infrastructure.PrismMasonManagementInfrastructureBase
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PrismMasonManagementDbContext _context;

        public Repository(PrismMasonManagementDbContext context)
        {
            _context = context;

        }
    }
}
