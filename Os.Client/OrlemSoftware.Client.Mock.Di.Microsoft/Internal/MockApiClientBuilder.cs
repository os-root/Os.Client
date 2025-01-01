using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Di.Microsoft.Internal;

internal class MockApiClientBuilder<TConfiguration> : IMockApiClientBuilder<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly IServiceCollection _services;

    public MockApiClientBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(
        Func<TRequest, bool> canRunCheck,
        Func<TRequest, Task<TResponse>> run
    )
        where TRequest : IApiClientRequest<TResponse>
        => AddRequestHandler(
            canRunCheck,
            (_, r) => run(r)
        );

    public IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(
            Func<TRequest, bool> canRunCheck,
            Func<IServiceProvider, TRequest, Task<TResponse>> run
        )
        where TRequest : IApiClientRequest<TResponse>
    {
        _services.AddSingleton<MockApiRequestHandler<TConfiguration>>(
            services => new MockApiRequestHandler<TConfiguration, TResponse>(
                services,
                x => x is TRequest tr && canRunCheck(tr),
                (s, x) => run(s, (TRequest)x)
            )
        );

        return this;
    }

    public IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(HandlingDelegate<TRequest, TResponse> handler)
        where TRequest : IApiClientRequest<TResponse>
    {
        _services.AddSingleton<MockApiRequestHandler<TConfiguration>>(
            services => new MockApiRequestHandler<TConfiguration, TResponse>(
                services,
                x => x is TRequest,
                (s, x) => handler(s, (TRequest)x)
            )
        );

        return this;
    }
}