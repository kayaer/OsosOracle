using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OsosOracle.MvcUI.RabbitMq
{
    public class Publisher
    {
        private readonly RabbitMqService _rabbitMqService;

        public Publisher(string queneName, string message)
        {
            _rabbitMqService = new RabbitMqService();

            using (var connection = _rabbitMqService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queneName, false, false, false, null);
                    channel.BasicPublish("", queneName, null, Encoding.UTF8.GetBytes(message));

                    Console.WriteLine("{0} queue'su üzerine, \"{1}\" mesajı yazıldı.", queneName, message);

                }
            }
        }
    }
}