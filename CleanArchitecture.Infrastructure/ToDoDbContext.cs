using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domian.Entities;

namespace CleanArchitecture.Infrastructure
{
    public partial class ToDoDbContext : DbContext
    {
        //就相当于你有一栋房子（ToDoDbContext），里面有1个房间：ToDoItems 房间：专门放待办事项；
        public ToDoDbContext()
        {
        }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {

        }

        public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
