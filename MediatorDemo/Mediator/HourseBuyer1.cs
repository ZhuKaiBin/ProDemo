namespace MediatorDemo.Mediator
{
    /// <summary>
    /// 买房的人
    /// </summary>
    public class HourseBuyer1 : People
    {
        public HourseBuyer1(HouseMediator houseMediator)
            : base(houseMediator) { }

        public void GetBuyHouseMsg(string msg)
        {
            Console.WriteLine($"买房1，获取到有人要卖房了:{msg}");
        }
    }
}
