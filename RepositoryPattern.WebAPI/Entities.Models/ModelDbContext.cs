using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RepositoryPattern.WebAPI.Entities.Models
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Owner>? Owners { get; set; }
        public DbSet<Account>? Accounts { get; set; }
    }
}
