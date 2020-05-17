using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RabbitMqConsumer
{
    public class Consumer
    {
        private readonly RabbitMqService _rabbitMqService;

        public Consumer(string queueName)
        {
            _rabbitMqService = new RabbitMqService();

            using (var connection = _rabbitMqService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        
                        using (StreamWriter w = File.AppendText("log.txt"))
                        {
                            Log(message, w);
                           
                        }
                        Console.WriteLine("{0} isimli queue üzerinden gelen mesaj: \"{1}\"", queueName, message);

                    };

                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                }
            }

             static void Log(string logMessage, TextWriter w)
            {
                //w.Write("\r\nLog Entry : ");
                //w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                //w.WriteLine("  :");
                w.WriteLine($"{logMessage},");
                //w.WriteLine("-------------------------------");
            }

        }
    }
}
