using Scalar2Sln_Domain.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scalar2Sln_Domain.ValueObjects;

namespace Scalar2Sln_Domain.Entities.TodoList
{
    public class TodoList: BaseAuditableEntity
    {
        public string? Title { get; set; }

        //EF Core 看到你写了一个引用类型属性（比如 public Colour Colour { get; set; }），
        //默认认为它是一个实体（Entity），而不是值对象（Value Object）。所以它想当然地：
        //“哦，你这是一个要建表的实体呀！那我得给它加主键！”
        public Colour Colour { get; set; } = Colour.White;

        public IList<TodoItem> Items { get;  set; } = new List<TodoItem>();
    }
}
