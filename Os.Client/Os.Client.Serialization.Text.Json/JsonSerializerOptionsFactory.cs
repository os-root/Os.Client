using System.Text.Json;
using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Serialization.Text.Json;

public sealed class JsonSerializerOptionsFactory<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly Action<TConfiguration, JsonSerializerOptions> _serializerSetup;
    private readonly IServiceProvider _serviceProvider;

    internal JsonSerializerOptionsFactory(Action<TConfiguration, JsonSerializerOptions> serializerSetup,
        IServiceProvider serviceProvider)
    {
        _serializerSetup = serializerSetup;
        _serviceProvider = serviceProvider;
    }

    public JsonSerializerOptions GetOptions()
    {
        var config = (TConfiguration)_serviceProvider.GetService(typeof(TConfiguration))!;
        var retv = new JsonSerializerOptions
        {
            TypeInfoResolver = JsonSerializerOptions.Default.TypeInfoResolver
        };
        _serializerSetup.Invoke(config, retv);
        return retv;
    }
}