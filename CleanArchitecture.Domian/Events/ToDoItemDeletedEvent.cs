using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domian.Events
{
    public class ToDoItemDeletedEvent:BaseEvent
    {

        public ToDoItem _item;

        public ToDoItemDeletedEvent(ToDoItem item)
        {
            _item = item;
        }
    }
}
