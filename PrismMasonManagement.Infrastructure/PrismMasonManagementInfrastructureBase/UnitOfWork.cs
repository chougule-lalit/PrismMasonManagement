﻿using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Infrastructure.PrismMasonManagementInfrastructureBase
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PrismMasonManagementDbContext _context;
        private Hashtable _repositories;


        public UnitOfWork(PrismMasonManagementDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}
