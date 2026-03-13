using QualityAutomation.Tests.Config;
using Serilog;
using Serilog.Events;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Configures and initializes Serilog logging for the test framework
/// </summary>
public static class LoggerSetup
{
    private static bool _isInitialized;
    private static readonly object _lock = new();

    /// <summary>
    /// Initializes the Serilog logger with configured settings
    /// </summary>
    public static void Initialize()
    {
        if (_isInitialized) return;

        lock (_lock)
        {
            if (_isInitialized) return;

            var logSettings = ConfigurationManager.Instance.LogSettings;
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logSettings.LogPath);

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            var logFileName = Path.Combine(logPath, $"TestLog_{DateTime.Now:yyyyMMdd}.log");
            var logLevel = ParseLogLevel(logSettings.LogLevel);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    logFileName,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}",
                    shared: true)
                .CreateLogger();

            _isInitialized = true;
            Log.Information("Logger initialized successfully");
        }
    }

    private static LogEventLevel ParseLogLevel(string level)
    {
        return level.ToLower() switch
        {
            "verbose" => LogEventLevel.Verbose,
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }

    /// <summary>
    /// Closes and flushes the logger
    /// </summary>
    public static void CloseAndFlush()
    {
        Log.Information("Closing logger");
        Log.CloseAndFlush();
        _isInitialized = false;
    }
}
