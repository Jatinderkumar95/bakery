using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace bakery
{
    public class ConfigSerilog
    {
        public static void Configure()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DevelopmentEnvironment;
            var isDevelopment = env == DevelopmentEnvironment;
            var defaultLoggingLevelSwitch = AddLoggingLevelSwitch("default", LogEventLevel.Warning);
            AddLoggingLevelSwitch("System", LogEventLevel.Warning);
            // create a LoggingLevelSwitch for each namespace that needs to be controlled independently
            AddLoggingLevelSwitch("Microsoft", LogEventLevel.Warning);
            AddLoggingLevelSwitch("Steeltoe", LogEventLevel.Warning);

            Configure(config =>
            {
                config
                    .MinimumLevel.ControlledBy(defaultLoggingLevelSwitch)
                    .Enrich.FromLogContext();
                    //.Enrich.WithHttpRequestUrlReferrer()
                    //.Enrich.WithVisitorId()
                    //.Enrich.WithClientIp()
                    //.Enrich.WithFullUrl()
                    //.Enrich.WithXForwardFor()
                    //.Enrich.WithLwp()
                    //.Enrich.WithUserDevice()
                    //.Enrich.WithCorrelation()
                    //.Destructure.UsingAttributes();

                foreach (var loggingLevelSwitch in LoggingLevelConfig.Where(x => x.Key != "default"))
                {
                    config.MinimumLevel.Override(loggingLevelSwitch.Key, loggingLevelSwitch.Value.loggingLevelSwitch);
                }


                if (isDevelopment)
                    config.WriteTo.Console(/*outputTemplate: DevelopmentEnvironmentOutputTemplate*/);
                else
                    config.WriteTo.Console(/*new RenderedCompactJsonFormatter()*/);
            });
        }
        private static LoggingLevelSwitch AddLoggingLevelSwitch(string source, LogEventLevel logEventLevel)
        {
            var logLevelSwitch = new LoggingLevelSwitch(logEventLevel);
            LoggingLevelConfig.Add(source, (logLevelSwitch, logEventLevel));
            return logLevelSwitch;
        }
        private static readonly IDictionary<string, (LoggingLevelSwitch loggingLevelSwitch, LogEventLevel configuredLogLevel)> LoggingLevelConfig =
            new ConcurrentDictionary<string, (LoggingLevelSwitch loggingLevelSwitch, LogEventLevel configuredLogLevel)>(StringComparer.InvariantCultureIgnoreCase);

        public static void Configure(Action<LoggerConfiguration> config)
        {
            if (_configuration == null)
            {
                _configuration = new LoggerConfiguration();
                config(_configuration);
                Log.Logger = _configuration.CreateLogger();
            }
        }
        private static LoggerConfiguration _configuration;

        public static string? DevelopmentEnvironment = "Development";
    }
}
