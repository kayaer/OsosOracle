using Listener.Entities;
using Listener.Helpers;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Listener
{
    public class ListenerWindowsService
    {
        static TcpServer server = new TcpServer(10002);
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListenerWindowsService));
        public ListenerWindowsService()
        {
            //Log 4 konfigurasyon
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = @"Logs\EventLog.txt";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            BasicConfigurator.Configure(hierarchy);

        }
        public void Start()
        {
            EntHamData rd = new EntHamData();
            rd.Data = Encoding.UTF8.GetBytes("test");
            rd.KonsSeriNo = "123";
            rd.Ip = "123";

            // AmrData.GetInstance().Kuyruk.Enqueue(rd);
            //RabbitMq kuyruga atılıyor readout dataları
            RabbitmqHelper.AddQueue(rd);

            server.StartListen();
        }
        public void Stop()
        {
            _log.Info("Servis durduruldu");
        }
    }
}
