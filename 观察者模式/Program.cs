using System;
using System.Xml.Linq;

namespace 观察者模式
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Subject subjectA = new ConcreteSubject();

            Observer observer = new ConctereObserve("张三", subjectA);
            Observer observer2 = new ConctereObserve("张三2", subjectA);
            Observer observer3 = new ConctereObserve("张三3", subjectA);
            Observer observer4 = new ConctereObserve("张三4", subjectA);

            subjectA.setState("更新了一集");

            subjectA.setState("又更新了一集");
        }
    }

    public interface Subject //目标
    {
        public void Attach(Observer observer);

        public void Detach(Observer observer);

        public void Notify();//状态改变后，通知所有观察者

        public string getState();

        public void setState(string state);//改变状态
    }

    public class ConcreteSubject : Subject
    {
        private string state;
        public List<Observer> Observers;

        public ConcreteSubject()
        {
            state = "未更新";
            Observers = new List<Observer>();
        }

        public string getState()
        {
            return state;
        }

        public void setState(string state)
        {
            this.state = state;
            Notify();
        }

        public void Attach(Observer observer)
        {
            Observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            Observers.Remove(observer);
        }

        public void Notify()
        {
            //遍历集合中的每个对象，通知更新
            foreach (Observer observer in Observers)
            {
                observer.update();
            }
        }
    }

    public interface Observer //观察者接口
    {
        public void update();//收到通知更新观察者状态
    }

    public class ConctereObserve : Observer
    {
        private string _name;
        private string _state;
        private Subject _subject;

        public ConctereObserve(string name, Subject subject)
        {
            _name = name;
            _subject = subject;

            this._subject = subject;
            subject.Attach(this);

            _state = subject.getState();
        }

        public void update()
        {
            Console.WriteLine(_name + "：收到通知");
            _state = _subject.getState();//让当前观察者的状态和改变之后的目标状态保持一致

            Console.WriteLine($"收到了更新内容：{_state}");
        }
    }
}