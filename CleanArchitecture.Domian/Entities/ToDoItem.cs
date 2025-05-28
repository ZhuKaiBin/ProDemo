using CleanArchitecture.Domian.Common;

namespace CleanArchitecture.Domian.Entities
{
    public class ToDoItem: BaseAuditableEntity
    {
        
        public required string Description { get; set; }
        public bool IsDone { get; set; }
    }
}
