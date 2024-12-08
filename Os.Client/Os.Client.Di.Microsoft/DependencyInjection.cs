using Microsoft.Extensions.DependencyInjection;
using Os.Client.Di.Microsoft.Internal;
using Os.Client.Interfaces;
using Os.Client.Internal;

namespace Os.Client.Di.Microsoft;

public static class DependencyInjection
{
    public static IApiClientBuilder<TConfiguration> AddApiClient<TApiClient, TStrategylessApiClient, TConfiguration>(this IServiceCollection services, TConfiguration configuration)
        where TApiClient : class, IApiClient<TConfiguration>
        where TStrategylessApiClient : class, IStrategylessApiClient<TConfiguration>
        where TConfiguration : class, IApiClientConfiguration
    {
        services.AddSingleton(configuration);
        services.AddTransient<TApiClient>();
        services.AddTransient<IApiClient<TConfiguration>>(s => s.GetRequiredService<TApiClient>());
        services.AddTransient<IApiClient>(s => s.GetRequiredService<TApiClient>());
        var httpClientBuilder = services.AddHttpClient<TApiClient>(cl => SetHttpClient(cl, configuration));

        services.AddTransient<TStrategylessApiClient>();
        services.AddTransient<IStrategylessApiClient<TConfiguration>>(s => s.GetRequiredService<TStrategylessApiClient>());
        var strategylessHttpClientBuilder = services.AddHttpClient<TStrategylessApiClient>(cl => SetHttpClient(cl, configuration));

        var retv = new ApiClientBuilder<TConfiguration>(new[] { httpClientBuilder, strategylessHttpClientBuilder }, services);
        return retv;
    }

    public static IApiClientBuilder<TConfiguration> AddApiClient<TConfiguration>(this IServiceCollection services, TConfiguration configuration)
        where TConfiguration : class, IApiClientConfiguration
        => services
            .AddApiClient<GenericApiClient<TConfiguration>, StrategylessGenericApiClient<TConfiguration>, TConfiguration>(configuration);

    private static void SetHttpClient(HttpClient cl, IApiClientConfiguration configuration)
    {
        cl.BaseAddress = new Uri(configuration.ApiBase);
        if (configuration.Timeout.HasValue)
            cl.Timeout = configuration.Timeout.Value;

        foreach (var baseHeader in configuration.DefaultHeaders)
            cl.DefaultRequestHeaders.Add(baseHeader.Key, baseHeader.Value);
    }
}