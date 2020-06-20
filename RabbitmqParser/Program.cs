using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace RabbitmqParser
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfig =>
            {
                hostConfig.SetServiceName("ParserService");
                hostConfig.SetDisplayName("Parser Service");
                hostConfig.RunAsLocalSystem();

                hostConfig.UseLog4Net();

                hostConfig.Service<ParserWindowsService>(serviceConfig =>
                {

                    serviceConfig.ConstructUsing(() => new ParserWindowsService());
                    serviceConfig.WhenStarted(s => s.Start());
                    serviceConfig.WhenStopped(s => s.Stop());
                });
            });
        }
    }
}
