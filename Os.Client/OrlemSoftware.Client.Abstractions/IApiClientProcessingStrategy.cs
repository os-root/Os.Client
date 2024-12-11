namespace OrlemSoftware.Client.Abstractions;

public interface IApiClientProcessingStrategy
{
    Task<bool> CheckCanApplyAsync(HttpRequestMessage requestMessage);
    Task<bool> CheckCanApplyAsync(HttpResponseMessage responseMessage);
    Task ApplyAsync(HttpRequestMessage requestMessage);
}

public interface IApiClientProcessingStrategy<TConfiguration> : IApiClientProcessingStrategy
    where TConfiguration : IApiClientConfiguration
{
}