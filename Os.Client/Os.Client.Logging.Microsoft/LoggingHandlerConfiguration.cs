using Microsoft.Extensions.Logging;

namespace Os.Client.Logging.Microsoft;

public record LoggingHandlerConfiguration
{
    public LogLevel HttpLogsLevel { get; init; }
}