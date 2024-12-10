using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client.Internal;

public class StrategylessGenericApiClient<TConfiguration> : GenericApiClient<TConfiguration>, IStrategylessApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public StrategylessGenericApiClient(
        HttpClient httpClient,
        TConfiguration configuration,
        IClientLoggerFactory<TConfiguration> loggerFactory,
        IResponseDeserializer<TConfiguration> responseDeserializer,
        IRequestSerializer<TConfiguration> requestSerializer)
        : base(httpClient,
            configuration,
            loggerFactory,
            new GenericResponseSuccessChecker<TConfiguration>(),
            Array.Empty<IApiClientProcessingStrategy<TConfiguration>>(),
            responseDeserializer, requestSerializer)
    {
    }
}