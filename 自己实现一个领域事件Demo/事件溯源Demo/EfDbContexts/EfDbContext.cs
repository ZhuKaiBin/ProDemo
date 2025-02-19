using EventSourceDemo.Entities;
using Microsoft.EntityFrameworkCore;
namespace EventSourceDemo.EfDbContexts
{
    public class EfDbContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<EventStore> EventStore { get; set; }  // 存储事件的表


        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 定义 EventStore 的表结构
            modelBuilder.Entity<EventStore>().ToTable("EventStore").HasKey(x => x.EventId);

            // 其他表的配置...
        }
    }
}
