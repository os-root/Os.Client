namespace OrlemSoftware.Client.Interfaces;

public interface IClientLoggerFactory<TConfiguration> : IClientLoggerFactory
    where TConfiguration : IApiClientConfiguration
{

}

public interface IClientLoggerFactory
{
    public IClientLogger CreateLogger();
    public IClientLogger CreateLogger<TCaller>();
    public IClientLogger CreateLogger<TCaller>(TCaller caller);
    public IClientLogger CreateLogger(Type callerType);
}