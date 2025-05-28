using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domian.Events
{
    public class ToDoItemCreatedEvent:BaseEvent
    {
        public ToDoItem _item;

        public ToDoItemCreatedEvent(ToDoItem item)
        {
            _item = item;
        }
    }
}
