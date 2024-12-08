namespace Os.Client.Interfaces;

public interface IApiClient
{
    Task<TResponse> SendAsync<TResponse>(BaseApiRequest<TResponse> request, CancellationToken cancellationToken);
    Task SendAsync(BaseApiRequest request, CancellationToken cancellationToken);
    Task<TResponse> SendAsync<TResponse>(BaseApiRequest<TResponse> request);
    Task SendAsync(BaseApiRequest request);
}

public interface IApiClient<TConfiguration> : IApiClient
    where TConfiguration : IApiClientConfiguration
{

}