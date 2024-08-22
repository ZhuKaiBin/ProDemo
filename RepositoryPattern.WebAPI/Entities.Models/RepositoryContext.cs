using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RepositoryPattern.WebAPI.Entities.Models
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Owner>? Owners { get; set; }
        public DbSet<Account>? Accounts { get; set; }
    }
}
