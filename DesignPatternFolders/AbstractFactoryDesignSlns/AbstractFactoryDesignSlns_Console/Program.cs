namespace AbstractFactoryDesignSlns_Console
{
    /// <summary>
    /// 抽象工厂设计模式（Abstract Factory Pattern）
    /// 1. 面向接口编程，不依赖具体实现(我只说我要一个“连接器”，但我不说你给我SQL的还是NoSQL的。)
    /// 2. 易于扩展，不影响旧代码(以后你想加 MongoDB、Oracle，只要多建几个子工厂就行，主逻辑代码不用改。)
    /// 
    /// 抽象工厂模式就像是一家工厂，它把“要造什么”这件事隐藏起来，客户端只负责下订单，而不是亲自动手造。
    /// 这样你要换货、换产线、扩展品类都轻松自如，耦合度低，扩展性强。    
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            IDatabaseFactory factory = new SQLDatabaseFactory();
            IDatabase sqlDatabase = factory.CreateDatabase();
            sqlDatabase.Connect();
            Console.WriteLine("Hello, World!");
        }
    }




    //工厂的抽象合同
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase();
    }

    //不同的子工厂
    public class SQLDatabaseFactory : IDatabaseFactory
    {
        public IDatabase CreateDatabase() => new SQLDataBase();
    }

    //不同的子工厂
    public class NoSQLDatabaseFactory : IDatabaseFactory
    {
        public IDatabase CreateDatabase() => new NoSQLDataBase();
    }


    //数据库连接器的接口,所有连接器都应该有一个“插入电源、连网”方法（Connect）
    public interface IDatabase
    {
        void Connect();
    }

    //不同的产品，执行具体的连接动作
    public class SQLDataBase : IDatabase
    {
        public void Connect() => Console.WriteLine("SQL数据库");
    }

    //不同的产品，执行具体的连接动作
    public class NoSQLDataBase : IDatabase
    {
        public void Connect() => Console.WriteLine("NoSQLDataBase数据库");
    }
}
