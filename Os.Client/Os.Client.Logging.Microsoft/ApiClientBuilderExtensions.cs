using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrlemSoftware.Client.Abstractions;
using OrlemSoftware.Client.Di.Microsoft;

namespace OrlemSoftware.Client.Logging.Microsoft;

public static class ApiClientBuilderExtensions
{
    public static IApiClientBuilder<TConfiguration> AddHttpLogging<TConfiguration>(this IApiClientBuilder<TConfiguration> builder, LogLevel httpLogsLevel)
        where TConfiguration : IApiClientConfiguration
    {
        builder.Services.AddSingleton(new LoggingHandlerConfiguration { HttpLogsLevel = httpLogsLevel });
        builder.Services.AddScoped<LoggingHandler>();
        builder.ConfigureHttpClient(x => x.ConfigurePrimaryHttpMessageHandler<LoggingHandler>());

        return builder;
    }

    public static IApiClientBuilder<TConfiguration> AddWebProxy<TConfiguration>(this IApiClientBuilder<TConfiguration> builder, IWebProxy proxy)
        where TConfiguration : IApiClientConfiguration
    {
        builder.AddDefaultHttpHandler<HttpClientHandler>(sp => new HttpClientHandler
        {
            Proxy = proxy
        });

        return builder;
    }

    public static IApiClientBuilder<TConfiguration> AddMsLogger<TConfiguration>(this IApiClientBuilder<TConfiguration> builder)
        where TConfiguration : IApiClientConfiguration
    {
        builder.Services.AddSingleton(typeof(ClientLoggingAdapter<>));
        builder.Services.AddSingleton(typeof(ClientLoggerFactory<>));
        builder.AddLogger<ClientLoggerFactory<TConfiguration>>();
        return builder;
    }
}