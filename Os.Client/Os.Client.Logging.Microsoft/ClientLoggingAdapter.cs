using Microsoft.Extensions.Logging;
using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Logging.Microsoft;

internal class ClientLoggingAdapter<T> : IClientLogger
{
    private readonly ILogger<T> _logger;

    public ClientLoggingAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void Log(ClientLogLevel level, string message, params object[] args)
    {
        var msLevel = GetLogLevel(level);
        _logger.Log(msLevel, message, args);
    }

    public void Log(ClientLogLevel level, Exception ex, string message, params object[] args)
    {
        var msLevel = GetLogLevel(level);
        _logger.Log(msLevel, ex, message, args);
    }

    private LogLevel GetLogLevel(ClientLogLevel logLevel)
    {
        return logLevel switch
        {
            ClientLogLevel.Debug => LogLevel.Debug,
            ClientLogLevel.Info => LogLevel.Information,
            ClientLogLevel.Warning => LogLevel.Warning,
            ClientLogLevel.Error => LogLevel.Error,
            ClientLogLevel.Critical => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
}