using System.ComponentModel.DataAnnotations.Schema;

namespace Scalar2Sln_Domain.Common
{
    public abstract class BaseEntity
    {

        public int Id { set; get; }

        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]//这个属性不需要映射到数据库表中的列     
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }


        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
