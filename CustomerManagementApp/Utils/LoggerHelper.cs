using log4net;
using log4net.Config;
using System;
using System.IO;

namespace CustomerManagementApp.Utils
{
    public static class LoggerHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoggerHelper));

        static LoggerHelper()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            Logger.Error(message, ex);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }
    }
}
