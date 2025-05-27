using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar2Sln_Domain.Common;

namespace Scalar2Sln_Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {

        private readonly IMediator _mediator;

        public DispatchDomainEventsInterceptor(IMediator mediator)
        {
            _mediator = mediator;
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

        //其实这里面最重要的就是对这个Context的逻辑处理，就是说马上就要保存数据库了，还有一些什么操作;
        //或者说是在保存数据库之后要做什么操作等等之类的；
        //
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
                await _mediator.Publish(domainEvent);
        }




    }
}
