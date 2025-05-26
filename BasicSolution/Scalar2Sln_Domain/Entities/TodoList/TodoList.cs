using Scalar2Sln_Domain.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Domain.Entities.TodoList
{
    public class TodoList: BaseAuditableEntity
    {
        public string? Title { get; set; }
        public Colour Colour { get; set; } = Colour.White;

        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
