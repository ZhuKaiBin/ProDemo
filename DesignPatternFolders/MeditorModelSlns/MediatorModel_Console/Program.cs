namespace MediatorModel_Console
{
    #region
    //中介者模式（Mediator Pattern）是一种行为型设计模式，它通过【引入一个中介者对象来简化对象之间的通信】。

    //中介者模式的主要目的是减少对象之间的直接交互，避免多个对象之间形成复杂的相互依赖关系。
    //通过中介者模式，所有对象的交互都通过一个中介者来进行，从而简化了对象之间的协作关系。

    //主要角色：
    //Mediator（中介者接口）：

    //定义一个方法，用于与具体的组件进行交互。中介者对象负责协调各个同事对象之间的交互。
    //ConcreteMediator（具体中介者）：

    //实现 Mediator 接口，维护各个同事对象的引用，并负责它们之间的交互。
    //Colleague（同事类）：

    //每个同事对象通过中介者与其他对象交互，而不是直接与其他同事对象交互。同事类通常会有一个指向中介者对象的引用。
    #endregion


    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建中介者（聊天室）
            IChatRoomMediator chatRoom = new ChatRoomMediator();

            // 创建用户并加入聊天室
            User user1 = new User("Alice", chatRoom);
            User user2 = new User("Bob", chatRoom);
            User user3 = new User("Charlie", chatRoom);

            chatRoom.AddUser(user1);
            chatRoom.AddUser(user2);
            chatRoom.AddUser(user3);

            // 用户发送消息
            user1.SendMessage("Hello everyone!");
            user2.SendMessage("Hi Alice!");
            user3.SendMessage("Hey, how's it going?");
        }
    }


    // 1.中介者接口：中介者接口定义了与同事对象交互的方法。
    public interface IChatRoomMediator
    {
        void SendMessage(string message, User user);
        void AddUser(User user);
    }

    // 2.具体中介者 具体的中介者类实现了中介者接口，负责处理同事对象之间的通信。
    public class ChatRoomMediator : IChatRoomMediator
    {
        private List<User> users;

        public ChatRoomMediator()
        {
            users = new List<User>();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void SendMessage(string message, User user)
        {
            foreach (var u in users)
            {
                // 排除发消息的用户本身
                if (u != user)
                {
                    u.ReceiveMessage(message);
                }
            }
        }
    }

    // 同事类：用户 同事类定义了具体的用户，每个用户都知道中介者，并通过中介者发送和接收消息。
    public class User
    {
        private IChatRoomMediator mediator;
        public string Name { get; set; }

        public User(string name, IChatRoomMediator mediator)
        {
            this.Name = name;
            this.mediator = mediator;
        }

        public void SendMessage(string message)
        {
            Console.WriteLine($"{Name} sends: {message}");
            mediator.SendMessage(message, this);  // 通过中介者发送消息
        }

        public void ReceiveMessage(string message)
        {
            Console.WriteLine($"{Name} received: {message}");
        }
    }




}
