namespace Os.Client.Interfaces;

public interface IStrategylessApiClient<TConfiguration> : IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{

}