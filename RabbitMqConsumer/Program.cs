using System;

namespace RabbitMqConsumer
{
    class Program
    {
        private static Consumer _consumer;
        static void Main(string[] args)
        {
            _consumer = new Consumer("ENTSATIS");
            Console.ReadKey();
        }
    }
}
