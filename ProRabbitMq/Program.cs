using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ProRabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "";
            factory.Password = "";
            factory.HostName = "";
            factory.UserName = "";


            using (IConnection conn= factory.CreateConnection())
            {
                using (IModel channel=conn.CreateModel())
                {
                    channel.QueueDeclare("queuename", false, true, false, null);

                    channel.ExchangeDeclare("exchangename", ExchangeType.Direct, true, false, null);

                    channel.QueueBind("queuename", "exchangename", string.Empty, null);



                    byte[] BYTE = Encoding.UTF8.GetBytes("bob");

                    channel.BasicPublish("exchangenama", string.Empty, basicProperties: null, body: BYTE);

                }
            }


            using (IConnection conn = factory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {

                    channel.QueueDeclare("queuename", false, true, false, null);
                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume("queuename",false,consumer);




                    var p= channel.CreateBasicProperties();
                    p.DeliveryMode = 2;



                    consumer.Received += (model,ea)=> {
                        var body = ea.Body;
                    };
                }
            }


        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
        }
    }



    
}
