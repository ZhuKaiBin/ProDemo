namespace CommandModel_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建接收者
            Light light = new Light();

            // 创建具体命令
            ICommand lightOn = new LightOnCommand(light);
            ICommand lightOff = new LightOffCommand(light);

            // 创建遥控器
            RemoteControl remote = new RemoteControl();

            // 按下开灯按钮
            remote.SetCommand(lightOn);
            remote.PressButton();

            // 按下关灯按钮
            remote.SetCommand(lightOff);
            remote.PressButton();
        }
    }

    // 命令接口 命令接口定义了一个执行操作的方法。
    public interface ICommand
    {
        void Execute();  // 执行命令
    }


    // Receiver 类接收者类 接收者类是真正执行具体操作的地方。在这个例子中，接收者是 Light 类，它包含了开灯和关灯的操作。
    public class Light
    {
        public void TurnOn()
        {
            Console.WriteLine("The light is ON");
        }

        public void TurnOff()
        {
            Console.WriteLine("The light is OFF");
        }
    }


    //3. ConcreteCommand 类：
    //具体命令类会封装具体的请求。这里我们有两个命令类，LightOnCommand 和 LightOffCommand，它们分别对应开灯和关灯操作。

    // 具体命令类 1: 开灯命令
    public class LightOnCommand : ICommand
    {
        private Light light;

        public LightOnCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.TurnOn();
        }
    }

    // 具体命令类 2: 关灯命令
    public class LightOffCommand : ICommand
    {
        private Light light;

        public LightOffCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.TurnOff();
        }
    }


    //调用者类 4. Invoker 类：
    //调用者类，发出命令的地方。它可以持有一个命令对象，并调用其 execute 方法来请求执行。
    public class RemoteControl
    {
        private ICommand command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public void PressButton()
        {
            command.Execute();  // 执行命令
        }
    }

}
