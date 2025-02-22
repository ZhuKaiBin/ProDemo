namespace BuilderModel_Console
{
    internal class Program
    {
        #region
        //建造者设计模式（Builder Pattern）是一种创建型设计模式，旨在使用多个步骤来构建一个复杂对象，
        //且该对象的构建过程可以独立于其组成部分的创建和表达方式。
        //该模式将对象的构建过程与它的表示分离，允许你使用相同的构建过程创建不同的表示。


        //主要角色：
        //    Builder（建造者）：定义了创建产品各个部件的抽象方法。
        //    ConcreteBuilder（具体建造者）：实现了 Builder 接口，构建各个部件，且提供了一个方法来返回最终构建的对象。
        //    Director（指挥者）：负责根据特定的顺序使用 Builder 来构建复杂的对象。它不关心具体的构建细节。
        //    Product（产品）：最终被构建的复杂对象。包含多个部件。

        //    比喻：
        //    可以将建造者模式比作建筑工地上的建筑师和工人。建筑师（Director）负责规划建筑的结构，
        //    而工人（ConcreteBuilder）负责具体的建造工作，最后将每一块砖、瓦、窗户等部分拼接成一座完整的建筑（Product）。
        //    通过这种方式，
        //    建筑师可以在不关注工人具体如何操作的情况下，快速且灵活地建造不同的房子。

        #endregion

        static void Main(string[] args)
        {
            // 选择建造者（建造不同类型的电脑）
            IComputerBuilder gamingBuilder = new GamingComputerBuilder();
            IComputerBuilder officeBuilder = new OfficeComputerBuilder();


            // 指挥者管理建造过程
            ComputerDirector director = new ComputerDirector(gamingBuilder);
            Computer gamingComputer = director.Construct();
            Console.WriteLine("Gaming Computer:");
            gamingComputer.ShowInfo();

            Console.WriteLine("---------------------");

            // 改为另一种电脑配置
            director = new ComputerDirector(officeBuilder);
            Computer officeComputer = director.Construct();
            Console.WriteLine("Office Computer:");
            officeComputer.ShowInfo();
        }
    }

    //要构建的复杂对象本身：复杂对象（产品）本身，可以是任何你要构建的对象。这里我们使用 Computer 作为产品类。
    public class Computer
    {
        public string CPU { get; set; }

        public string GPU { get; set; }

        public string RAM { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine($"CPU: {CPU}");
            Console.WriteLine($"GPU: {GPU}");
            Console.WriteLine($"RAM: {RAM}");
        }

    }


    // 1.抽象建造者
    public interface IComputerBuilder
    {
        void BuildCPU();
        void BuildGPU();
        void BuildRAM();
        Computer GetResult();
    }



    // 2. ConcreteBuilder 类：
    //具体建造者实现了 IComputerBuilder，并提供了构建每个部件的实现。
    public class GamingComputerBuilder : IComputerBuilder
    {
        //这里是要构建具体的哪种产品
        private Computer _computer = new Computer();
        public void BuildCPU()
        {
            _computer.CPU = "Intel i9";
            Console.WriteLine("Building CPU for Gaming Computer: Intel i9");
        }

        public void BuildGPU()
        {
            _computer.GPU = "NVIDIA RTX 3090";
            Console.WriteLine("Building GPU for Gaming Computer: NVIDIA RTX 3090");
        }

        public void BuildRAM()
        {
            _computer.RAM = "32GB DDR4";
            Console.WriteLine("Building RAM for Gaming Computer: 32GB DDR4");
        }

        public Computer GetResult()
        {
            return _computer;
        }

    }

    public class OfficeComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        public void BuildCPU()
        {
            _computer.CPU = "Intel i5";
            Console.WriteLine("Building CPU for Office Computer: Intel i5");
        }

        public void BuildGPU()
        {
            _computer.GPU = "Integrated GPU";
            Console.WriteLine("Building GPU for Office Computer: Integrated GPU");
        }

        public void BuildRAM()
        {
            _computer.RAM = "8GB DDR4";
            Console.WriteLine("Building RAM for Office Computer: 8GB DDR4");
        }

        public Computer GetResult()
        {
            return _computer;
        }
    }



    // 3. Director 类：
    //指挥者负���使用具体建造者来构建复杂对象。它不关心具体的构建细节，只负责按照特定的顺序来调用具体建造者的方法。
    public class ComputerDirector
    {
        private IComputerBuilder _builder;
        public ComputerDirector(IComputerBuilder builder)
        {
            _builder = builder;
        }
        public void Construct()
        {
            _builder.BuildCPU();
            _builder.BuildGPU();
            _builder.BuildRAM();
        }
    }

}
