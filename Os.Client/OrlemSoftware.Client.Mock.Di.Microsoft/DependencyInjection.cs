using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Abstractions;
using OrlemSoftware.Client.Mock.Di.Microsoft.Internal;

namespace OrlemSoftware.Client.Mock.Di.Microsoft;

public static class DependencyInjection
{
    public static IMockApiClientBuilder<TConfiguration> AddMockApiClient<TConfiguration>(this IServiceCollection services)
        where TConfiguration : IApiClientConfiguration
    {
        services.AddSingleton<MockApiClient<TConfiguration>>();
        services.AddSingleton<IApiClient<TConfiguration>>(s => s.GetRequiredService<MockApiClient<TConfiguration>>());
        var mockBuilder = new MockApiClientBuilder<TConfiguration>(services);
        return mockBuilder;
    }
}