using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock;

public class MockApiRequestHandler<TConfiguration, TResponse> : MockApiRequestHandler<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public MockApiRequestHandler(
            IServiceProvider serviceProvider,
            Func<IApiClientRequest<TResponse>, bool> canRun,
            Func<IServiceProvider, IApiClientRequest<TResponse>, Task<TResponse>> run
        ) : base(
            serviceProvider, 
            x => x is IApiClientRequest<TResponse> r && canRun(r),
            async (s, x) => await run(s, (IApiClientRequest<TResponse>)x)
        )
    {
    }
}

public class MockApiRequestHandler<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    protected IServiceProvider ServiceProvider { get; }
    private readonly Func<IApiClientRequest, bool> _canRun;
    private readonly Func<IServiceProvider, IApiClientRequest, Task<object?>> _run;

    protected MockApiRequestHandler(
        IServiceProvider serviceProvider,
        Func<IApiClientRequest, bool> canRun,
        Func<IServiceProvider, IApiClientRequest, Task<object?>> run
    )
    {
        ServiceProvider = serviceProvider;
        _canRun = canRun;
        _run = run;
    }

    public MockApiRequestHandler(
        IServiceProvider serviceProvider,
        Func<IApiClientRequest, bool> canRun,
        Func<IServiceProvider, IApiClientRequest, Task> run
    )
    : this(serviceProvider, canRun, 
        async (s, x) =>
        {
            await run(s, x);
            return null;
        })
    {
    }

    public virtual bool CheckCanRun(IApiClientRequest request)
        => _canRun(request);

    public virtual async Task<object?> Run(IApiClientRequest request)
    {
        try
        {
            return await _run(ServiceProvider, request);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ApiException(e, 500, null);
        }
    }

    public async Task<TResponse> Run<TResponse>(IApiClientRequest<TResponse> request)
        => (TResponse)(await Run((IApiClientRequest)request))!;
}