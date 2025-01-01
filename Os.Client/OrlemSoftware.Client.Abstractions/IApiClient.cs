namespace OrlemSoftware.Client.Abstractions;

public interface IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request, CancellationToken cancellationToken);
    Task Send(IApiClientRequest request, CancellationToken cancellationToken);
    Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request);
    Task Send(IApiClientRequest request);
}