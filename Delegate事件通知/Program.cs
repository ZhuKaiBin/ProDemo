namespace Delegate事件通知
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gameManager = new GameManager();

            //把相关事件绑定到这个【主干】上
            var sysResource = new ResourceSys(gameManager);
            var MusicManager = new MusicManager(gameManager);

            //启动
            gameManager.StartGame();
            gameManager.EndGame();
        }
    }

    //这个里面其实就是声明了一个【主干】的绳，谁要有什么关联动作，到时候引用我，绑定到我身上
    public class GameManager
    {
        public Action OnGameStart;
        public Action OnGameEnd;

        //public event Action OnGameStart;  //在这里加上event这个主要是限制调用Invoke(),这个方法只能在GameManager类中调用，其他地方都不能调用
        //public event Action OnGameEnd;

        //通过调用这个方法，开始执行里面一系列绑定的东西
        public void StartGame()
        {
            //这个是开始执行
            OnGameStart?.Invoke();
        }

        public void EndGame()
        {
            OnGameEnd?.Invoke();
        }
    }

    public class ResourceSys
    {
        private GameManager gameManager;

        //构造函数：是将ResourceSystem要做的事情绑定到【主干】上去
        public ResourceSys(GameManager gameManager)
        {
            this.gameManager = gameManager;

            gameManager.OnGameStart += loadResource;
            gameManager.OnGameEnd += ReleaseSource;

            //gameManager.OnGameStart?.Invoke();
        }

        public void loadResource()
        {
            Console.WriteLine("游戏开始...加载资源数据");
        }

        public void ReleaseSource()
        {
            Console.WriteLine("游戏结束...释放资源数据");
        }
    }

    public class MusicManager
    {
        private GameManager gameManager;

        public MusicManager(GameManager gameManager)
        {
            this.gameManager = gameManager;

            gameManager.OnGameStart += StartMusic;
            gameManager.OnGameEnd += StopMusic;
        }

        public void StartMusic()
        {
            Console.WriteLine("游戏开始...背景音乐起.....");
        }

        public void StopMusic()
        {
            Console.WriteLine("游戏结束...背景音乐停....");
        }
    }
}