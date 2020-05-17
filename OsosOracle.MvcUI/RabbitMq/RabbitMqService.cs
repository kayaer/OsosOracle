using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.RabbitMq
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