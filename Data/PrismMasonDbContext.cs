using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismMasonManagement.Models;

namespace PrismMasonManagement.Data
{
    public class PrismMasonDbContext : IdentityDbContext
    {
        public DbSet<Item> Items { get; set; }
        public PrismMasonDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}