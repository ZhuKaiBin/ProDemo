using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Interceptors
{
    /// <summary>
    /// 领域事件的分发拦截器
    /// </summary>
    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        //领域事件的分布器，根本还是在于MediR，用来分发和接收

        private readonly IMediator _mediator;
        public DispatchDomainEventsInterceptor(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

            return base.SavingChanges(eventData, result);

        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null)
                return;

            var entities = context.ChangeTracker
                             .Entries<BaseEntity>()
                             .Where(e => e.Entity.DomainEvents.Any())
                             .Select(e => e.Entity);

            // 把所有实体的领域事件打平成一个 List（复制了一份到 domainEvents 变量中）。
            //❗️注意：这是一个副本！已经把事件值拷贝出来了！
            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();


            //把原来的实体对象上挂载的 DomainEvents 清空。
            //此时，domainEvents 本身是不受影响的，因为它是上一步 .ToList() 得到的 值副本。
            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }

    }
}
