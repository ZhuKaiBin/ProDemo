using System.Threading.Channels;

namespace AbstractFactoryDesignPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1.跟工厂说，我要SQL的数据库，只需要指定想要的某种类型的数据库就行了，后面的在抽象工厂里封装好
            IDatabaseFactory factory = new SQLDatabaseFactory();
            IDatabase sqlDatabase = factory.CreateDatabase();
            sqlDatabase.Connect();
        }
    }

    //抽象工厂模式提供了一个接口，用于创建相关或依赖对象的族，而无需指定其具体类型。
    //此模式允许【客户端使用抽象类】而不是具体类来创建对象族。



    // Abstract Factory(对外的一个工厂窗口)，对外的实例由工厂来产生，外部不需要再 New对象
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase();
    }


    public class SQLDatabaseFactory : IDatabaseFactory
    {
        public IDatabase CreateDatabase() => new SQLDataBase();
    }

    public class NoSQLDatabaseFactory : IDatabaseFactory
    {
        public IDatabase CreateDatabase() => new NoSQLDataBase();
    }


    // Abstract Product
    public interface IDatabase
    {
        void Connect();
    }

    public class SQLDataBase : IDatabase
    {
        public void Connect() => Console.WriteLine("SQL数据库");
    }

    public class NoSQLDataBase : IDatabase
    {
        public void Connect() => Console.WriteLine("NoSQLDataBase数据库");
    }



}
