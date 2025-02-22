using static System.Net.Mime.MediaTypeNames;

namespace ProxyModel_Console
{
    //代理模式（Proxy Pattern）是一种结构型设计模式，目的是为其他对象提供一种代理，以控制对这个对象的访问。
    //代理模式常用于延迟初始化、权限控制、远程代理等场景。
    //代理模式通过引入【代理对象】来【间接】访问真实对象，可以实现对真实对象的控制或增强。


    //代理模式通常包括以下角色：
    //Subject：定义了真实对象和代理对象共同遵循的接口。
    //RealSubject：真实对象，实现了 Subject 接口，包含具体的业务逻辑。
    //Proxy：代理对象，实现了 Subject 接口，控制对 RealSubject 的访问。


    //代理模式的类型：
    //虚拟代理（Virtual Proxy）：延迟加载，只有在需要时才创建和加载真实对象。
    //远程代理（Remote Proxy）：用于远程访问的代理，客户端通过代理访问远程服务器上的对象。
    //保护代理（Protective Proxy）：控制对真实对象的访问，通常用于权限管理。
    //缓存代理（Cache Proxy）：缓存数据，避免重复的计算或访问。


    #region 代理模式的优点
    //控制访问：可以控制对真实对象的访问（例如延迟加载、权限控制等）。
    //增强功能：可以在不修改真实对象的情况下，在代理中添加额外的功能（如缓存、日志记录等）。
    //隔离复杂性：客户端通过代理访问对象，代理可以隐藏真实对象的复杂性。
    //代理模式适用于那些需要控制对真实对象访问、延迟加载或做一些权限管理的场景。在实际应用中，远程代理、虚拟代理、保护代理和缓存代理等都是常见的使用场景。

    #endregion

    internal class Program
    {
        //客户端通过 ProxyImage 来访问图像，而不直接访问 RealImage。代理对象控制图像加载的过程，并且在必要时才加载图像。
        static void Main(string[] args)
        {
            IImage image1 = new ProxyImage("image1.jpg");
            IImage image2 = new ProxyImage("image2.jpg");
            // 第一次调用时，加载并显示图像
            image1.Display();

            // 第二次调用时，直接显示图像（没有重新加载）
            image1.Display();

            // 调用另一个图像
            image2.Display();
        }
    }


    //1. Subject 接口（Subject）
    //首先，我们定义一个 Subject 接口，作为真实对象和代理对象的共同接口。
    public interface IImage
    {
        void Display();
    }

    //2. RealSubject 类（RealSubject）
    //RealSubject 类实现了 IImage 接口，表示一个具体的图像对象，它包含真实的图像加载逻辑。

    // 真实对象
    public class RealImage : IImage
    {
        private string filename;

        public RealImage(string filename)
        {
            this.filename = filename;
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            Console.WriteLine($"Loading image: {filename}");
        }

        public void Display()
        {
            Console.WriteLine($"Displaying image: {filename}");
        }
    }


    // 3. Proxy 类（Proxy）
    //代理类也实现了 IImage 接口，但它控制对 RealImage 的访问。代理对象在第一次请求时才会创建 RealImage，并提供其接口方法。
    public class ProxyImage : IImage
    {
        private RealImage realImage;
        private string filename;

        public ProxyImage(string filename)
        {
            this.filename = filename;
        }

        public void Display()
        {
            // 如果真实对象还没有创建，则创建它
            if (realImage == null)
            {
                realImage = new RealImage(filename);
            }

            realImage.Display();
        }
    }




}
