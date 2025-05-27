using Scalar2Sln_Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Domain.Entities.TodoList
{
    public class TodoItem : BaseAuditableEntity
    {
        public int ListId { get; set; }

        public string? Title { get; set; }

        public string? Note { get; set; }

        public PriorityLevel Priority { get; set; }

        public DateTime? Reminder { get; set; }

        private bool _done;
        public bool Done
        {
            get => _done;
            set
            {
                if (value && !_done)
                {
                    //AddDomainEvent(new TodoItemCompletedEvent(this));
                }

                _done = value;
            }
        }

        public TodoList List { get; set; } = null!;
    }
}
