using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Interceptors
{
    public class AuditableEntityInterceptor: SaveChangesInterceptor
    {
        private readonly TimeProvider _dateTime;
        public AuditableEntityInterceptor(TimeProvider dateTime)
        {
            _dateTime = dateTime;
        }

        // 
        /// <summary>
        /// 1. 保存流程开始
        /// </summary>
        /// <param name="eventData">提供当前 DbContext 实例的上下文信息（你可以通过 eventData.Context 访问 DbContext 本体）</param>
        /// <param name="result">表示拦截器是否想要短路（跳过）保存流程，或者继续按原计划进行</param>
        /// <returns></returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {

            Console.WriteLine("Before SaveChanges");

            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData">提供当前 DbContext 实例的上下文信息（你可以通过 eventData.Context 访问 DbContext 本体）</param>
        /// <param name="result">表示拦截器是否想要短路（跳过）保存流程，或者继续按原计划进行</param>
        /// <param name="cancellationToken">支持取消操作，比如 Web 请求超时</param>
        /// <returns></returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // 2. 成功保存后
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            Console.WriteLine("After SaveChanges Success");
            return base.SavedChanges(eventData, result);
        }


        //3.保存失败
        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            Console.WriteLine("SaveChanges Failed");
            base.SaveChangesFailed(eventData);
        }

        // 4. 异步取消
        public override void SaveChangesCanceled(DbContextEventData eventData)
        {
            Console.WriteLine("SaveChanges Canceled");
            base.SaveChangesCanceled(eventData);
        }



        public void UpdateEntities(DbContext? context)
        {
            if (context == null)
                return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                var utcNow = _dateTime.GetUtcNow();
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {

                    if (entry.State == EntityState.Added)
                    {
                        //entry.Entity.CreatedBy = _user.Id;
                        entry.Entity.Created = utcNow;
                    }
                    //entry.Entity.LastModifiedBy = _user.Id;
                    entry.Entity.LastModified = utcNow;
                }

                if (entry.State == EntityState.Deleted)
                {
                    //entry.Entity.LastModifiedBy = _user.Id;
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
