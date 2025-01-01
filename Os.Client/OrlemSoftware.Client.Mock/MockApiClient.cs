using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock;

public class MockApiClient<TConfiguration> : IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly IEnumerable<MockApiRequestHandler<TConfiguration>> _requestHandlers;

    public MockApiClient(IEnumerable<MockApiRequestHandler<TConfiguration>> requestHandlers)
    {
        _requestHandlers = requestHandlers;
    }

    public async Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request, CancellationToken cancellationToken) 
        => await Send(request);

    public async Task Send(IApiClientRequest request, CancellationToken cancellationToken)
    {
        await Send(request);
    }

    public async Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request)
    {
        var factory = _requestHandlers.FirstOrDefault(x => x.CheckCanRun(request));
        if (factory == null)
            ThrowApiEx();

        return await factory!.Run(request);
    }

    public async Task Send(IApiClientRequest request)
    {
        var factory = _requestHandlers.FirstOrDefault(x => x.CheckCanRun(request));
        if (factory == null)
            ThrowApiEx();

        await factory!.Run(request);
    }

    private void ThrowApiEx()
    {
        throw new ApiException(500, "Unknown request");
    }
}