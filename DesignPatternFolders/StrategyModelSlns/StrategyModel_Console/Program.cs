namespace StrategyModel_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Strategy concreteStrategyA = new ConcreteStrategyA();

            Context context = new Context(concreteStrategyA);
            context.ContextInterface(3, 4);

            Strategy concreteStrategyB = new ConcreteStrategyB();
            context = new Context(concreteStrategyB);
            context.ContextInterface(3, 4);
        }
    }


    interface Strategy
    {
        //定义一个算法接口
        void AlgorithmInterface(int a, int b);
    }

    public class ConcreteStrategyA : Strategy
    {
        public void AlgorithmInterface(int a, int b)
        {
            Console.WriteLine("算法A实现:" a + b);
        }
    }

    public class ConcreteStrategyB : Strategy
    {
        public void AlgorithmInterface(int a, int b)
        {
            Console.WriteLine("算法B实现:" a * b);
        }
    }

    public class Context
    {
        private Strategy strategy;
        public Context(Strategy strategy)
        {
            this.strategy = strategy;
        }
        public void ContextInterface(int a, int b)
        {
            strategy.AlgorithmInterface(a, b);
        }
    }
}
