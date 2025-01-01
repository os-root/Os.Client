using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Di.Microsoft;

public interface IMockApiClientBuilder<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(
        Func<TRequest, bool> canRunCheck,
        Func<TRequest, Task<TResponse>> run
    )
        where TRequest : IApiClientRequest<TResponse>;

    IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(
        Func<TRequest, bool> canRunCheck,
        Func<IServiceProvider, TRequest, Task<TResponse>> run
    )
        where TRequest : IApiClientRequest<TResponse>;
}