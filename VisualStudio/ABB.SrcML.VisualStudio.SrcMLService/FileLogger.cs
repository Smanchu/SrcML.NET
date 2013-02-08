﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace ABB.SrcML.VisualStudio.SrcMLService {
    public class FileLogger {
        public static ILog CreateFileLogger(string loggerName, string filePath) {
            var appender = CreateFileAppender(loggerName + "Appender", filePath);
            AddAppender(loggerName, appender);
            return LogManager.GetLogger(loggerName);
        }

        public static ILog DefaultLogger {
            get {
                return LogManager.GetLogger("DefaultLogger");
            }
        }

        private static void AddAppender(string loggerName, IAppender appender) {
            var log = LogManager.GetLogger(loggerName);
            var logger = (Logger)log.Logger;

            logger.AddAppender(appender);
            logger.Repository.Configured = true;
        }

        private static IAppender CreateFileAppender(string name, string fileName) {
            var appender = new FileAppender {
                Name = name,
                File = fileName,
                AppendToFile = false,
                ImmediateFlush = true,
                LockingModel = new FileAppender.MinimalLock()
            };

            var layout = new PatternLayout {
                ConversionPattern = "%date %-5level %logger - %message%newline"
            };
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }

        static FileLogger() {
            var fileInfo = new FileInfo(Assembly.GetCallingAssembly().Location);
            var defaultLogPath = Path.Combine(fileInfo.DirectoryName, "SrcML.NETService" + Guid.NewGuid() + ".log");
            CreateDefaultLogger(defaultLogPath);
        }

        private static void CreateDefaultLogger(string defaultLoggerLogFile) {
            string configurationContent =
                @"<?xml version='1.0'?>
				<log4net>
					<appender name='DefaultFileAppender' type='log4net.Appender.FileAppender'>
						<file value='" + defaultLoggerLogFile + @"' />
						<appendToFile value='false' />
						<lockingModel type='log4net.Appender.FileAppender+MinimalLock' />
						<maximumFileSize value='100KB' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%date %-5level %logger - %message%newline' />
						</layout>
					</appender>

                    <logger name='DefaultLogger' additivity='false'>
                        <level value='ALL' />
                        <appender-ref ref='DefaultFileAppender' />
                    </logger>
    
					<root>
						<level value='DEBUG' />
						<appender-ref ref='DefaultFileAppender' />
					</root>
				</log4net>";
            XmlConfigurator.Configure(new MemoryStream(Encoding.Default.GetBytes(configurationContent)));
        }
    }
}