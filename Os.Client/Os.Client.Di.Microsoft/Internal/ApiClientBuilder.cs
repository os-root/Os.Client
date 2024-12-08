using Microsoft.Extensions.DependencyInjection;
using Os.Client.Interfaces;
using Os.Client.Internal;

namespace Os.Client.Di.Microsoft.Internal;

internal class ApiClientBuilder<TConfiguration> : IApiClientBuilder<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly IReadOnlyCollection<IHttpClientBuilder> _httpClientBuilders;

    public IServiceCollection Services { get; }

    public ApiClientBuilder(IReadOnlyCollection<IHttpClientBuilder> httpClientBuilders,
        IServiceCollection services)
    {
        _httpClientBuilders = httpClientBuilders;
        Services = services;
        RegisterFailsafeDefaults(Services);
    }

    private static void RegisterFailsafeDefaults(IServiceCollection services)
    {
        services.AddSingleton(NullClientLoggerFactory<TConfiguration>.Instance);
        services.AddSingleton<IClientLoggerFactory<TConfiguration>>(s => s.GetRequiredService<NullClientLoggerFactory<TConfiguration>>());
        services.AddSingleton<IClientLoggerFactory>(s => s.GetRequiredService<NullClientLoggerFactory<TConfiguration>>());

        services.AddSingleton(DefaultClientJson<TConfiguration>.Instance);
        services.AddSingleton<IRequestSerializer<TConfiguration>>(s => s.GetRequiredService<DefaultClientJson<TConfiguration>>());
        services.AddSingleton<IRequestSerializer>(s => s.GetRequiredService<DefaultClientJson<TConfiguration>>());
        services.AddSingleton<IResponseDeserializer<TConfiguration>>(s => s.GetRequiredService<DefaultClientJson<TConfiguration>>());
        services.AddSingleton<IResponseDeserializer>(s => s.GetRequiredService<DefaultClientJson<TConfiguration>>());

        services.AddTransient<GenericResponseSuccessChecker<TConfiguration>>();
        services.AddTransient<IResponseSuccessChecker<TConfiguration>>(s => s.GetRequiredService<GenericResponseSuccessChecker<TConfiguration>>());
        services.AddTransient<IResponseSuccessChecker>(s => s.GetRequiredService<GenericResponseSuccessChecker<TConfiguration>>());

        services.AddTransient<HttpClientHandler>();
        services.AddTransient<HttpMessageHandler>(s=>s.GetRequiredService<HttpClientHandler>());
    }

    public IApiClientBuilder<TConfiguration> AddDefaultHttpHandler<THandler>() where THandler : HttpMessageHandler
    {
        Services.AddTransient<THandler>();
        Services.AddTransient<HttpMessageHandler>(s => s.GetRequiredService<THandler>());
        return this;
    }
    public IApiClientBuilder<TConfiguration> AddDefaultHttpHandler<THandler>(Func<IServiceProvider, THandler> factory) 
        where THandler : HttpMessageHandler
    {
        Services.AddTransient<THandler>(factory);
        Services.AddTransient<HttpMessageHandler>(s => s.GetRequiredService<THandler>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> ConfigureHttpClient(Action<IHttpClientBuilder> httpConfiguration)
    {
        foreach (var httpClientBuilder in _httpClientBuilders)
            httpConfiguration(httpClientBuilder);

        return this;
    }

    public IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>()
        where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>
    {
        Services.AddSingleton<TStrategy>();
        Services.AddSingleton<IApiClientProcessingStrategy<TConfiguration>>(s => s.GetRequiredService<TStrategy>());
        Services.AddSingleton<IApiClientProcessingStrategy>(s => s.GetRequiredService<TStrategy>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>(Func<IServiceProvider, TStrategy> factory)
        where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>
    {
        Services.AddSingleton<TStrategy>(factory);
        Services.AddSingleton<IApiClientProcessingStrategy<TConfiguration>>(s => s.GetRequiredService<TStrategy>());
        Services.AddSingleton<IApiClientProcessingStrategy>(s => s.GetRequiredService<TStrategy>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddApiClientProcessingStrategy<TStrategy>(TStrategy strategy)
        where TStrategy : class, IApiClientProcessingStrategy<TConfiguration>
    {
        Services.AddSingleton<TStrategy>(strategy);
        Services.AddSingleton<IApiClientProcessingStrategy<TConfiguration>>(s => s.GetRequiredService<TStrategy>());
        Services.AddSingleton<IApiClientProcessingStrategy>(s => s.GetRequiredService<TStrategy>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddResponseSuccessChecker<TChecker>()
        where TChecker : class, IResponseSuccessChecker<TConfiguration>
    {
        Services.AddTransient<TChecker>();
        Services.AddTransient<IResponseSuccessChecker<TConfiguration>>(s => s.GetRequiredService<TChecker>());
        Services.AddTransient<IResponseSuccessChecker>(s => s.GetRequiredService<TChecker>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddLogger<TLoggerFactory>()
        where TLoggerFactory : class, IClientLoggerFactory<TConfiguration>
    {
        Services.AddSingleton<IClientLoggerFactory<TConfiguration>>(s => s.GetRequiredService<TLoggerFactory>());
        Services.AddSingleton<IClientLoggerFactory>(s => s.GetRequiredService<TLoggerFactory>());
        Services.AddSingleton<IClientLogger>(s => s.GetRequiredService<TLoggerFactory>().CreateLogger());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddRequestSerializer<TSerializer>()
        where TSerializer : class, IRequestSerializer<TConfiguration>
    {
        Services.AddSingleton<TSerializer>();
        Services.AddSingleton<IRequestSerializer<TConfiguration>>(s => s.GetRequiredService<TSerializer>());
        Services.AddSingleton<IRequestSerializer>(s => s.GetRequiredService<TSerializer>());
        return this;
    }

    public IApiClientBuilder<TConfiguration> AddResponseDeserializer<TDeserializer>()
        where TDeserializer : class, IResponseDeserializer<TConfiguration>
    {
        Services.AddSingleton<TDeserializer>();
        Services.AddSingleton<IResponseDeserializer<TConfiguration>>(s => s.GetRequiredService<TDeserializer>());
        Services.AddSingleton<IResponseDeserializer>(s => s.GetRequiredService<TDeserializer>());
        return this;
    }
}