using Common.Logging;
using System;
using System.Collections.Generic;

namespace Logging.Common.Extensions
{
    public static class LogExtensions
    {
        private static Dictionary<string, LogLevel> levelsOverriding;
        private static bool levelsOverridingEnabled;

        static LogExtensions()
        {
            levelsOverriding = new Dictionary<string, LogLevel>(); //{ { "D7DF0CFF-5CBC-4029-80E1-C4B651252FC1", LogLevel.Warn } };//levelsOverridingConfiguration.Rules
            // todo = false если конфигурации нет или явно задан аттрибут disable
            levelsOverridingEnabled = true;//levelsOverriding.Rules.Count > 0 && levelsOverridingConfiguration.Enabled;
        }

        public static void WriteLog(this ILog logger, LogLevel logLevel, Func<string> log, string logEntryIdentifier = null)
        {

            LogLevel level = GetLogLevel(logLevel, logEntryIdentifier);

            if (CheckLevel(logger, level))
            {
                var posfix = logEntryIdentifier == null ? string.Empty : $"{Environment.NewLine}LogEntryIdentifier:[{logEntryIdentifier}]";

                InnerWriteLog(logger, log() + posfix, level);
            }
        }

        public static void WriteLog(this ILog logger, LogLevel logLevel, Func<string> log, Exception ex, string logEntryIdentifier = null)
        {

            LogLevel level = GetLogLevel(logLevel, logEntryIdentifier);

            if (CheckLevel(logger, level))
            {
                var posfix = logEntryIdentifier == null ? string.Empty : $"{Environment.NewLine}LogEntryIdentifier:[{logEntryIdentifier}]";

                InnerWriteLog(logger, log() + posfix, level, ex);
            }
        }

        public static void Trace(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Trace, log, logEntryIdentifier);
        }

        public static void Debug(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Debug, log, logEntryIdentifier);
        }

        public static void Info(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Info, log, logEntryIdentifier);
        }

        public static void Info(this ILog logger, Func<string> log, Exception exception, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Info, log, exception, logEntryIdentifier);
        }

        public static void Warn(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Warn, log, logEntryIdentifier);
        }

        public static void Warn(this ILog logger, Func<string> log, Exception exception, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Warn, log, exception, logEntryIdentifier);
        }

        public static void Error(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Error, log, logEntryIdentifier);
        }

        public static void Error(this ILog logger, Func<string> log, Exception exception, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Error, log, exception, logEntryIdentifier);
        }

        public static void Fatal(this ILog logger, Func<string> log, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Fatal, log, logEntryIdentifier);
        }

        public static void Fatal(this ILog logger, Func<string> log, Exception exception, string logEntryIdentifier = null)
        {
            logger.WriteLog(LogLevel.Fatal, log, exception, logEntryIdentifier);
        }

        private static void InnerWriteLog(ILog logger, string message, LogLevel level, Exception ex)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    logger.Trace(message, ex);
                    break;
                case LogLevel.Debug:
                    logger.Debug(message, ex);
                    break;
                case LogLevel.Info:
                    logger.Info(message, ex);
                    break;
                case LogLevel.Warn:
                    logger.Warn(message, ex);
                    break;
                case LogLevel.Error:
                    logger.Error(message, ex);
                    break;
                case LogLevel.Fatal:
                    logger.Fatal(message, ex);
                    break;
                default:
                    throw new ArgumentException($"Неизвестный уровень логгирования {level}");
            }
        }

        private static void InnerWriteLog(ILog logger, string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    logger.Trace(message);
                    break;
                case LogLevel.Debug:
                    logger.Debug(message);
                    break;
                case LogLevel.Info:
                    logger.Info(message);
                    break;
                case LogLevel.Warn:
                    logger.Warn(message);
                    break;
                case LogLevel.Error:
                    logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentException($"Неизвестный уровень логгирования {level}");
            }
        }

        private static bool CheckLevel(ILog logger, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return logger.IsTraceEnabled;
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Info:
                    return logger.IsInfoEnabled;
                case LogLevel.Warn:
                    return logger.IsWarnEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return logger.IsFatalEnabled;
                case LogLevel.All:
                    return true;
                case LogLevel.Off:
                    return false;
                default:
                    throw new ArgumentException($"Неизвестный уровень логгирования {level}");
            }
        }

        private static LogLevel GetLogLevel(LogLevel defaultLevel, string logEntryIdentifier)
        {
            if (logEntryIdentifier == null || !levelsOverridingEnabled)
            {
                return defaultLevel;
            }

            LogLevel level;

            return levelsOverriding.TryGetValue(logEntryIdentifier, out level) ? level : defaultLevel;
        }
    }
}
