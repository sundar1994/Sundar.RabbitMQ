using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Producer
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
            //声明一个队列
            channel.QueueDeclare("hello", true, false, false, null);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            Thread.Sleep(1000);

            for (int i = 0; i < 300; i++)
            {
                string input = $"hello{i}";
                var sendBytes = Encoding.UTF8.GetBytes(input);
                channel.BasicPublish("", "hello", null, sendBytes);
                Console.WriteLine(input);
            }

            channel.Close();
            connection.Close();

            Console.ReadKey();
        }
    }
}
