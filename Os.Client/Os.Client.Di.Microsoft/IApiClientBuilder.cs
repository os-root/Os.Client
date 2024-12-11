using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Di.Microsoft;

public interface IApiClientBuilder<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public IServiceCollection Services { get; }
    IApiClientBuilder<TConfiguration> AddDefaultHttpHandler<THandler>()
        where THandler: HttpMessageHandler;
    IApiClientBuilder<TConfiguration> AddDefaultHttpHandler<THandler>(Func<IServiceProvider, THandler> factory)
        where THandler : HttpMessageHandler;
    IApiClientBuilder<TConfiguration> ConfigureHttpClient(Action<IHttpClientBuilder> httpConfiguration);
    IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>() where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>(Func<IServiceProvider, TStrategy> factory) where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>(TStrategy strategy) where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddResponseSuccessChecker<TChecker>()
        where TChecker : class, IResponseSuccessChecker<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddLogger<TLoggerFactory>()
        where TLoggerFactory : class, IClientLoggerFactory<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddRequestSerializer<TSerializer>()
        where TSerializer : class, IRequestSerializer<TConfiguration>;
    IApiClientBuilder<TConfiguration> AddResponseDeserializer<TDeserializer>()
        where TDeserializer : class, IResponseDeserializer<TConfiguration>;
}