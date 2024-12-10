namespace OrlemSoftware.Client.Interfaces;

public interface IStrategylessApiClient<TConfiguration> : IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{

}