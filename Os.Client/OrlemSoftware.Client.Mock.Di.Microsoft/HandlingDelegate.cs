using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Di.Microsoft;

public delegate Task<TResponse> HandlingDelegate<in TRequest, TResponse>(IServiceProvider services, TRequest request)
    where TRequest : IApiClientRequest<TResponse>;
public delegate Task HandlingDelegate<in TRequest>(IServiceProvider services, TRequest request)
    where TRequest : IApiClientRequest;