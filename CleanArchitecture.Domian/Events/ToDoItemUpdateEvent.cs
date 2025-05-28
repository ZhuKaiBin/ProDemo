using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domian.Events
{
    public class ToDoItemUpdateEvent:BaseEvent
    {
        public ToDoItem _item;

        public ToDoItemUpdateEvent(ToDoItem item)
        {
            _item = item;
        }
    }
}
