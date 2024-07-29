namespace MediatorDemo.Mediator
{
    public class AnJuKe : HouseMediator
    {
        private HourseBuyer1 buyer1;
        private HourseSeller1 seller1;

        public HourseBuyer1 Buyer1
        {
            set { buyer1 = value; }
        }
        public HourseSeller1 Seller1
        {
            set { seller1 = value; }
        }

        public override void SendHouseMsg(string msg, People people)
        {
            //如果people是卖方的，那么买房子的人就会收到信息
            if (people == seller1)
            {
                buyer1.GetBuyHouseMsg(msg);
            }
            else
            {
                //如果是买房子的，那么卖房的人就会收到
                seller1.GetSellerHouseMsg(msg);
            }
        }
    }
}
