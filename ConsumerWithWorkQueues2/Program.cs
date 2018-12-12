using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ConsumerWithWorkQueues
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "test",//用户名
                Password = "test123",//密码
                HostName = "192.168.173.22",//rabbitmq ip
                VirtualHost = "HoyoTestVirtualHost"
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();
            /*
             第一个参数: 可接收消息的大小(一般写死为0)
             第二个参数：处理消息最大的数量
             第三个参数：是不是针对整个Connection的
             */
            channel.BasicQos(0, 1, false);

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                Console.WriteLine($"工作队列2收到消息： {message}");
                Thread.Sleep(1000);
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //启动消费者 设置为手动应答消息
            channel.BasicConsume("hello", false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();

        }
    }
}
