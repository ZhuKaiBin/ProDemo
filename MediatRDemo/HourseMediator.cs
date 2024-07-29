using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRDemo
{
    /// <summary>
    /// 房子中介者
    /// </summary>
    public abstract class HourseMediator
    {
        public abstract void SendHourseMsg(string msg, People people);
    }

    public abstract class People
    {
        //无论买房还是卖房，都是要和中介关联的，这样就关联起来了，通过构造函数
        protected HourseMediator _mediator;

        public People(HourseMediator mediator)
        {
            _mediator = mediator;
        }

        public void SendMsg(string msg)
        {
            _mediator.SendHourseMsg(msg, this);
        }
    }

    public class HourseSeller : People
    {
        public HourseSeller(HourseMediator mediator)
            : base(mediator) { }

        public void GetBuyerMsg(string msg)
        {
            Console.WriteLine($"卖房子的人获取到信息{msg}");
        }
    }

    public class HorseBuyer : People
    {
        public HorseBuyer(HourseMediator mediator)
            : base(mediator) { }

        public void GetSellerMsg(string msg)
        {
            Console.WriteLine($"买房子的人获取到信息{msg}");
        }
    }

    public class Ajk : HourseMediator
    {
        private HorseBuyer buyer;
        private HourseSeller seller;

        public HorseBuyer Buyer
        {
            set => buyer = value;
        }
        public HourseSeller Seller
        {
            set => seller = value;
        }

        public override void SendHourseMsg(string msg, People people)
        {
            if (people == seller)
            {
                buyer.GetSellerMsg(msg);
            }

            if (people == buyer)
            {
                seller.GetBuyerMsg(msg);
            }
        }
    }
}
