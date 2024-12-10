using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client.Internal;

public class GenericApiClient<TConfiguration> : BaseApiClient, IApiClient<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    protected TConfiguration Configuration { get; }

    public GenericApiClient(
        HttpClient httpClient,
        TConfiguration configuration,
        IClientLoggerFactory<TConfiguration> loggerFactory,
        IResponseSuccessChecker<TConfiguration> responseSuccessChecker,
        IEnumerable<IApiClientProcessingStrategy<TConfiguration>> processingStrategies,
        IResponseDeserializer<TConfiguration> responseDeserializer,
        IRequestSerializer<TConfiguration> requestSerializer)
        : base(httpClient, processingStrategies, responseSuccessChecker, loggerFactory, responseDeserializer, requestSerializer)
    {
        Configuration = configuration;
    }
}