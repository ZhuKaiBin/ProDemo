using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Context
{
    public class MesDbContext : IdentityDbContext<IdentityUser>
    {
        public MesDbContext()
        { }

        public MesDbContext(DbContextOptions<MesDbContext> options)
        : base(options)  // 将 options 传递给基类 DbContext
        {
        }
    }
}