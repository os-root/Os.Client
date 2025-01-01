using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock;

public class MockApiRequestHandler<TConfiguration, TResponse> : MockApiRequestHandler<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly IServiceProvider _sp;
    private readonly Func<IApiClientRequest<TResponse>, bool> _canRun;
    private readonly Func<IServiceProvider, IApiClientRequest<TResponse>, Task<TResponse>> _run;

    public MockApiRequestHandler(
        IServiceProvider sp,
        Func<IApiClientRequest<TResponse>, bool> canRun, 
        Func<IServiceProvider, IApiClientRequest<TResponse>, Task<TResponse>> run
        )
    {
        _sp = sp;
        _canRun = canRun;
        _run = run;
    }

    public override bool CheckCanRun(IApiClientRequest request)
        => _canRun((IApiClientRequest<TResponse>)request);

    public override async Task<object> Run(IApiClientRequest request)
    {
        try
        {
            return (await _run(_sp, (IApiClientRequest<TResponse>)request))!;
        }
        catch (Exception e)
        {
            throw new ApiException(e, 500, null);
        }
    }
}

public abstract class MockApiRequestHandler<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public abstract bool CheckCanRun(IApiClientRequest request);
    public abstract Task<object> Run(IApiClientRequest request);

    public async Task<TResponse> Run<TResponse>(IApiClientRequest<TResponse> request)
        => (TResponse) await Run((IApiClientRequest) request);
}