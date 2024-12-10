using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client.Logging.Microsoft;

internal class ClientLoggerFactory<TConfiguration> : IClientLoggerFactory<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly IServiceProvider _sp;

    public ClientLoggerFactory(IServiceProvider sp)
    {
        _sp = sp;
    }

    public IClientLogger CreateLogger()
        => CreateLogger(typeof(ClientLoggerFactory<TConfiguration>));

    public IClientLogger CreateLogger<TCaller>()
        => CreateLogger(typeof(TCaller));

    public IClientLogger CreateLogger<TCaller>(TCaller caller) 
        => CreateLogger<TCaller>();

    public IClientLogger CreateLogger(Type callerType)
    {
        var finalType = typeof(ClientLoggingAdapter<>).MakeGenericType(callerType);
        var retv = _sp.GetRequiredService(finalType);
        return (IClientLogger)retv;
    }
}