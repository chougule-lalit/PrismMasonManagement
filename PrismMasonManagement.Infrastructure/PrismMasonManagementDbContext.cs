using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Core.Entities;
using PrismMasonManagement.Core.PrismMasonManagementCoreBase;
using PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrismMasonManagement.Infrastructure
{
    public class PrismMasonManagementDbContext : IdentityDbContext
    {
        private readonly ICurrentUser _currentUser;

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public PrismMasonManagementDbContext(DbContextOptions options,
            ICurrentUser currentUser) : base(options)
        {
            _currentUser = currentUser;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessSave()
        {
            var currentTime = DateTime.UtcNow;
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && (e.Entity is FullAuditedEntity<int> || e.Entity is FullAuditedEntity<Guid>)))
            {
                var isInteger = item.Entity is FullAuditedEntity<int>;
                if (isInteger)
                {
                    var entidad = item.Entity as FullAuditedEntity<int>;
                    entidad.CreationTime = currentTime;
                    entidad.CreatorId = _currentUser.Id;
                }
                else
                {
                    var entidad = item.Entity as FullAuditedEntity<Guid>;
                    entidad.CreationTime = currentTime;
                    entidad.CreatorId = _currentUser.Id;
                }
            }

            foreach (var item in ChangeTracker.Entries()
                .Where(predicate: e => e.State == EntityState.Modified && (e.Entity is FullAuditedEntity<int> || e.Entity is FullAuditedEntity<Guid>)))
            {
                var isInteger = item.Entity is FullAuditedEntity<int>;
                if (isInteger)
                {
                    var entidad = item.Entity as FullAuditedEntity<int>;
                    entidad.LastModificationTime = currentTime;
                    entidad.LastModifierId = _currentUser.Id;
                }
                else
                {
                    var entidad = item.Entity as FullAuditedEntity<Guid>;
                    entidad.LastModificationTime = currentTime;
                    entidad.LastModifierId = _currentUser.Id;
                }
            }
        }
    }
}