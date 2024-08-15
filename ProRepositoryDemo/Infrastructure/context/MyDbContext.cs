using Microsoft.EntityFrameworkCore;
using ProRepositoryDemo.Models;

namespace ProRepositoryDemo.Infrastructure.context;

// MyDbContext 是一个继承自 DbContext 的类，用于与数据库进行交互。它包含了数据库连接和模型配置的逻辑，
// 是 Entity Framework Core 的核心部分。这个类允许你定义和管理数据库中的表、关系和其他细节。
// MyDbContext 就像是一个桥梁，连接你的应用程序和数据库，让两者可以互相沟通和协作。它也像是一个管家，负责管理数据库中的各种资源。
public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }
    //无参构造函数就像是一个空的信封，里面没有任何东西（配置信息）。而有参构造函数就像是一个带有详细地址的信封，确保信可以准确送达目的地（数据库）。
    //这个构造函数接收配置选项，用于指定【数据库连接和其他配置细节】。在实际使用中，更常用有参构造函数，因为它可以提供更灵活的配置。
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
        
    }

    //配置数据库连接
    //OnConfiguring 方法就像是一个GPS导航系统，它告诉你的应用程序如何找到并连接到数据库。
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(
            "Server=localhost;Database=TestDbms;User ID=prozkb;Password=123456;" +
                          "TrustServerCertificate=True;");

    //OnModelCreating 方法用于进一步配置数据库模型的行为，比如定义表的名称、列的数据类型、关系等。
    //在这个例子中，它调用了一个部分方法 OnModelCreatingPartial，方便你在其他地方进一步扩展模型配置。
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 将实体类 Account 映射到数据库表 "Users"
        modelBuilder.Entity<Account>()
            .ToTable("Users");
        
        // 将属性 UserName 映射到数据库列 "user_name"
        // modelBuilder.Entity<Account>()
        //     .Property(a => a.UserName)
        //     .HasColumnName("user_name");
        
        
        // 设置 Account 实体的主键为 Id 属性
        // modelBuilder.Entity<Account>()
        //     .HasKey(a => a.Id);
        
        // 为 UserName 属性创建索引
        // modelBuilder.Entity<Account>()
        //     .HasIndex(a => a.UserName)
        //     .HasDatabaseName("Index_UserName");
        
        
        // 配置 Account 和 Role 之间的一对多关系
        // modelBuilder.Entity<Account>()
        //     .HasMany(a => a.Roles)  // 一个账户有多个角色
        //     .WithOne(r => r.Account) // 一个角色对应一个账户
        //     .HasForeignKey(r => r.AccountId); // 外键设置为 Role 表中的 AccountId 列
        
        //    // 忽略某个属性，不将其映射到数据库
        // modelBuilder.Entity<Account>()
        //     .Ignore(a => a.TemporaryProperty);
        //
        // // 忽略整个实体类，不将其映射到数据库
        // modelBuilder.Ignore<AuditLog>();
        OnModelCreatingPartial(modelBuilder);
    }
    
    //OnModelCreating 方法就像是一个设计师，负责布置你的数据库模型的布局。
    //OnModelCreatingPartial 就像是设计师预留的一部分，允许你以后再来添加或修改布局。
    
    // 生成的 OnModelCreatingPartial 方法通常没有方法体，是因为它是为你提供一个扩展点。如果你不需要进一步的自定义配置，方法体可以保持为空。
    // 如果你需要在 OnModelCreating 的基础上做进一步的配置，可以在同一个类的另一个部分中提供 OnModelCreatingPartial 的实现。
    
    
    // 在 OnModelCreatingPartial 中添加任何额外的配置，这些配置通常是特定于某些模块或场景的，或者是你不希望混入主要的 OnModelCreating 方法中的代码
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
