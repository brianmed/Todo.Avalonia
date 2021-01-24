using System;
using System.IO;
using System.Text.RegularExpressions;

using Serilog;
using Serilog.Events;

namespace Todo.AppCtx
{
    public class LoggingCtx
    {
        public static ILogger LogApp { get; internal set;}

        public static ILogger LogSql { get; internal set;}

        public static void Init()
        {
            LogApp = InitLogging("App");

            LogSql = InitLogging("Sql");
        }

        private static ILogger InitLogging(string ns)
        {
            ILogger log;

            string sql = Regex.Replace((Environment.GetEnvironmentVariable($"{nameof(Todo)}_{ns}_LOGGING".ToUpper())
                ?? Environment.GetEnvironmentVariable($"{nameof(Todo)}_DEFAULT_LOGGING".ToUpper())
                ?? String.Empty),
                "^$", "None");

            if (sql == "None") {
                log = Serilog.Core.Logger.None;
            } else {
                string appSqlFile = Path.Combine(Program.ConfigCtx.LogDirectory, $"{nameof(Todo)}-{ns}.log");
                Console.WriteLine($"Initializing {ns} Log: {sql} {appSqlFile}");

                LoggerConfiguration logConfiguration = new LoggerConfiguration();

                try
                {
                    logConfiguration = (LoggerConfiguration)(logConfiguration
                        .MinimumLevel.GetType()
                        .GetMethod(Enum.Parse<LogEventLevel>(sql).ToString())
                        .Invoke(logConfiguration.MinimumLevel, null));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Please use a minimum logging level of None, Verbose, Debug, Information, Warning, Error, Fatal: {ex.Message}");
                }

                log = logConfiguration
                    .Enrich.FromLogContext()
                    .WriteTo.File(appSqlFile,
                        fileSizeLimitBytes: 1024 * 1024 * 256,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 4)
                    .CreateLogger();

                log.Information($"Started {ns} Logging");
            }

            return log;
        }
    }
}