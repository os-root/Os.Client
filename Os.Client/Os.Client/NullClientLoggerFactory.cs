using Os.Client.Interfaces;

namespace Os.Client;

public class NullClientLoggerFactory<TConfiguration> : IClientLoggerFactory<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public static IClientLoggerFactory Instance { get; } = new NullClientLoggerFactory<TConfiguration>();
    private static IClientLogger NullLogger { get; } = new NullClientLogger();

    private class NullClientLogger : IClientLogger
    {
        public void Log(ClientLogLevel level, string message, params object[] args)
        {

        }

        public void Log(ClientLogLevel level, Exception ex, string message, params object[] args)
        {

        }
    }

    private NullClientLoggerFactory()
    {

    }

    public IClientLogger CreateLogger()
        => NullLogger;
    public IClientLogger CreateLogger<TCaller>()
        => NullLogger;
    public IClientLogger CreateLogger<TCaller>(TCaller caller)
        => NullLogger;
    public IClientLogger CreateLogger(Type callerType)
        => NullLogger;
}