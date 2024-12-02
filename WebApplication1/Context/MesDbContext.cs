using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class MesDbContext : IdentityDbContext<UserCustom>
    {
        public MesDbContext()
        { }

        public MesDbContext(DbContextOptions<MesDbContext> options)
        : base(options)  // 将 options 传递给基类 DbContext
        {
        }
    }
}