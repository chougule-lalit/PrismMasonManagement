using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Core.Entities;

namespace PrismMasonManagement.Infrastructure
{
    public class PrismMasonDbContext : IdentityDbContext
    {
        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        
        public PrismMasonDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}