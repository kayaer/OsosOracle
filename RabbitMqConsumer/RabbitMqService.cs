using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMqConsumer
{
    public class RabbitMqService
    {
        // localhost üzerinde kurulu olduğu için host adresi olarak bunu kullanıyorum.
        private readonly string _hostName = "localhost";

        public IConnection GetRabbitMQConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = _hostName };
            return connectionFactory.CreateConnection();
        }

    }
}
