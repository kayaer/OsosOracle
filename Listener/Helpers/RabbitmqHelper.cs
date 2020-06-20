using Newtonsoft.Json;
using Listener.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace Listener.Helpers
{
    public static class RabbitmqHelper
    {
        public static void AddQueue(EntHamData entHamData)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("HamData", false, false, false, null);
                    channel.BasicPublish("", "HamData", null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entHamData)));
                }
            }

        }

        public static void GetQueue(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var hamdata = JsonConvert.DeserializeObject<EntHamData>(message);
                        var data = Encoding.UTF8.GetString(hamdata.Data);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                   
                }
            }
        }


    }
}
