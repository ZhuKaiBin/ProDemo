using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Scalar2Sln_Domain.Entities.TodoList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scalar2Sln_Domain.ValueObjects;

namespace Scalar2Sln_Infrastructure.Data.Configurations
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            //OwnsOne 的作用：
            //告诉 EF Core：Colour 是 TodoList 的一部分，不是单独的一张表。

            //OwnsOne 可以直译为“拥有一个”
            builder
                .OwnsOne(b => b.Colour);
        }
    }
}
