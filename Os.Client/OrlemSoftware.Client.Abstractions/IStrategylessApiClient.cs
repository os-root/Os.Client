namespace OrlemSoftware.Client.Abstractions;

public interface IStrategylessApiClient<TConfiguration> : IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{

}