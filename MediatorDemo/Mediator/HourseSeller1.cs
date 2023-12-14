namespace MediatorDemo.Mediator
{
    /// <summary>
    /// 卖房的人
    /// </summary>
    public class HourseSeller1 : People
    {
        public HourseSeller1(HouseMediator houseMediator) : base(houseMediator)
        {
        }

        public void GetSellerHouseMsg(string msg)
        {
            Console.WriteLine($"卖房1，获取到有人买房:{msg}");
        }
    }
}
