namespace MediatorDemo.Mediator
{
    /// <summary>
    /// 将买房者和卖房者都抽象化，都是人
    /// </summary>
    public abstract class People
    {
        protected HouseMediator _houseMediator;
        protected People(HouseMediator houseMediator) { _houseMediator = houseMediator; }


        /// <summary>
        /// 用来发送消息，买房者用买发布买房信息；卖房者用来发布卖房
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            _houseMediator.SendHouseMsg(msg, this);
        }
    }
}
