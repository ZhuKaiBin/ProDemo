using System;

namespace ObserverPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //示例一个up住
            var upZhu = new UpZhu();

            var user1 = new User();
            var user2 = new User();

            //1和2都关注了up住
            upZhu.Sub(user1);
            upZhu.Sub(user2);

            upZhu.State = "更新了一个新的视频"; // 

            upZhu.Cancel(user1);//1号取关了
            upZhu.State = "Another State"; // Only observer2 will be notified
        }
    }



    //这是一个订阅者的服务指南，规定它需要有一个方法来接收和处理新闻更新。
    public interface IUser
    {
        //用户用来接收到up住的更新
        void ReceiveUpdate(string state);
    }


    /// <summary>
    /// 订阅的观众
    /// </summary>
    public class User : IUser
    {
        private string _info;

        //用于接收up住更新的内容
        public void ReceiveUpdate(string state)
        {
            _info = state;
            Console.WriteLine($"收到了Up主的更新: {_info}");
        }
    }



    public interface IBiliBili
    {
        //加关注
        void Sub(IUser observer);

        //取消关注
        void Cancel(IUser observer);

        //通知更新
        void Notity();
    }

    /// <summary>
    /// 一个新闻发言人
    /// </summary>
    public class UpZhu : IBiliBili
    {
        //up住要知道  都是哪些人关注了我
        private List<IUser> _observers = new List<IUser>();

        //新闻发言人的_state，记录者最新的发布内容
        private string _state;

        //State是上面给我的最新消息，只有State被改变了，证明就有新消息了，我也有要通知给订阅我的人了
        public string State
        {
            get => _state;
            set
            {
                _state = value;
                Notity();
            }
        }

        public void Sub(IUser observer) => _observers.Add(observer);
        public void Cancel(IUser observer) => _observers.Remove(observer);


        //告诉多有的人，这个state更新了
        public void Notity()
        {
            //通知到所有关注我的人；这里其实就是把Iuser给引入进来了，因为IUser里有方法，这个它参数出入进来，就已经是把所有的信息分发给所有继承IUser的人了
            //
            foreach (var observer in _observers)
            {
                observer.ReceiveUpdate(_state);
            }
        }
    }

}
