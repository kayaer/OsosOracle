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

namespace RabbitmqParser
{
    public class ParserWindowsService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ParserWindowsService));
        public ParserWindowsService()
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
            RabbitmqService rabbitmqService = new RabbitmqService();
            rabbitmqService.Consume();
            _log.Info("Servis Çalıştırıldı");
        }
        public void Stop()
        {
            _log.Info("Servis durduruldu");
        }
    }
}
