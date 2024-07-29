using System;
using System.Collections;
using System.Collections.Generic;

namespace AOP2
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 静态代理
            Order order = new Order { Amount = 2000, Number = 200 };

            OrderHandleProxy orderHandleInterceptor = new OrderHandleProxy(new OrderHandle());
            orderHandleInterceptor.Submit(order);
            #endregion
        }
    }

    public interface IOrderHandle
    {
        void Submit(Order order);
    }

    public class OrderHandle : IOrderHandle
    {
        public void Submit(Order order)
        {
            Console.WriteLine("提交订单");
        }
    }

    public class OrderHandleProxy : IOrderHandle
    {
        private IOrderHandle _orderHandle;

        public OrderHandleProxy(IOrderHandle orderHandle)
        {
            _orderHandle = orderHandle;
        }

        public void Submit(Order order)
        {
            PreSubmit(order);
            _orderHandle.Submit(order);
            AfterSubmit(order);
        }

        private void PreSubmit(Order order)
        {
            Console.WriteLine("提交订单前进行数据校验");
        }

        private void AfterSubmit(Order order)
        {
            Console.WriteLine("提交订单后进行日志记录");
        }
    }

    public class Order
    {
        /// <summary>
        /// 金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public int Price { get; set; }
    }
}
