using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfig =>
            {
                hostConfig.SetServiceName("ListenerService");
                hostConfig.SetDisplayName("Listener Service");
                hostConfig.RunAsLocalSystem();

                hostConfig.UseLog4Net();

                hostConfig.Service<ListenerWindowsService>(serviceConfig =>
                {

                    serviceConfig.ConstructUsing(() => new ListenerWindowsService());
                    serviceConfig.WhenStarted(s => s.Start());
                    serviceConfig.WhenStopped(s => s.Stop());
                });
            });
        }
    }
}
