using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Di.Microsoft;

public interface IMockApiClientBuilder<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest, TResponse>(HandlingDelegate<TRequest, TResponse> handler)
        where TRequest : IApiClientRequest<TResponse>;

    IMockApiClientBuilder<TConfiguration> AddRequestHandler<TRequest>(HandlingDelegate<TRequest> handler)
        where TRequest : IApiClientRequest;
}