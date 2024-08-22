using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SpecificationPattern
{

    /*
     1。对于在何处放置查询、排序和分页逻辑的问题，域驱动设计的一个解决方案是使用规范。规范设计模式描述对象中的查询。
     
     */


    internal class Program
    {
        static void Main(string[] args)
        {


        }
    }

    #region

    //通用规范接口
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }

    //通用规范实现（基类）
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();


        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }

    //通过规范（Specification）模式，你可以在不同的具体类中定义不同的查询逻辑，并且这些逻辑是可组合和可复用的。
    //传递的表达式 b => b.Id == basketId 的作用：它定义了筛选条件（过滤条件），用于查询特定的 Basket 实体，确保只返回 Id 等于 basketId 的 Basket。
    public class BasketWithItemsSpecification : BaseSpecification<Basket>
    {
        public BasketWithItemsSpecification(int basketId) : base(b => b.Id == basketId)
        {
            AddInclude(b => b.Items);
        }
        public BasketWithItemsSpecification(string buyerId)
            : base(b => b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }

    /*
     b => b.Id == basketId 这个表达式本质上是一个 Expression<Func<Basket, bool>>，它描述了一个 Lambda 表达式的结构，而不是具体的可执行代码。
    这种表达式树在使用 LINQ 查询时，会被解释并转化为 SQL 语句。
    在实际使用中，这是通过 Entity Framework 等 ORM 框架来实现的。

    当你使用 LINQ to Entities（即使用 LINQ 查询数据库）时，Entity Framework 会接收这个表达式树，并将其转换为等效的 SQL 查询。


    何时转化为 SQL
    这个转换过程发生在你对查询执行操作时，比如调用 ToList(), FirstOrDefault(), Count() 等方法时。
    以 ToList() 为例，当你调用这个方法时，LINQ 查询才会被真正执行，这时 Entity Framework 会把表达式树转化为 SQL 语句并发送给数据库。

     */

    #endregion
}
