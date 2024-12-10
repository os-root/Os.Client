using Microsoft.Extensions.Logging;

namespace OrlemSoftware.Client.Logging.Microsoft;

public record LoggingHandlerConfiguration
{
    public LogLevel HttpLogsLevel { get; init; }
}