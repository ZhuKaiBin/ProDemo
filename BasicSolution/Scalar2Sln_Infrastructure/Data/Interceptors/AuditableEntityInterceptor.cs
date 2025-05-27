using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scalar2Sln_Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar2Sln_Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Scalar2Sln_Infrastructure.Data.Interceptors
{
    //SaveChangesInterceptor 全是虚方法
    public class AuditableEntityInterceptor: SaveChangesInterceptor
    {

        private readonly IUser _user;
        private readonly TimeProvider _dateTime;

        public AuditableEntityInterceptor(IUser user, TimeProvider timeProvider)
        {
            _user = user;
            _dateTime = timeProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        //其实这里面最重要的就是对这个Context的逻辑处理，就是说马上就要保存数据库了，还有一些什么操作;
        //或者说是在保存数据库之后要做什么操作等等之类的；        //
        public void UpdateEntities(DbContext? context)
        {
            if (context == null)
                return;

            //遍历所有 继承自 BaseAuditableEntity 的实体，这些实体表示支持审计信息（例如有 Created, LastModified 字段）。
            //ChangeTracker.Entries<T>() 会返回所有被跟踪的实体，通常是本次变更涉及到的。
            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var utcNow = _dateTime.GetUtcNow();
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = _user.Id;
                        entry.Entity.Created = utcNow;
                    }
                    entry.Entity.LastModifiedBy = _user.Id;
                    entry.Entity.LastModified = utcNow;
                }
            
            }

        }

    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
         entry.References.Any(r =>
             r.TargetEntry != null &&
             r.TargetEntry.Metadata.IsOwned() &&
             (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

}
