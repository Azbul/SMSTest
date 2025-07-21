namespace Part3.Logging; 

using Microsoft.Extensions.Logging;
using Serilog;

using Logging = Microsoft.Extensions.Logging;

/// <summary>
/// Логгер.
/// </summary>
public static class Logger
{
    public static Logging.ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

    private static ILoggerFactory LoggerFactory { get; } = Logging.LoggerFactory.Create(builder =>
    {
        var logFilePath = $"test-sms-console-app-{DateTime.Now:yyyyMMdd}.log";
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.File(logFilePath)
            .WriteTo.Console()
            .CreateLogger();

        builder.AddSerilog(logger);
    });

}
