using System.Net.Mime;
using System.Text.Json;
using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client.Serialization.Text.Json;

public sealed class MsJson<TConfiguration> : IRequestSerializer<TConfiguration>, IResponseDeserializer<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    private readonly JsonSerializerOptions _serializerOptions;
    public string ContentType => MediaTypeNames.Application.Json;

    internal MsJson(JsonSerializerOptionsFactory<TConfiguration> serializerOptionsFactory)
    {
        _serializerOptions = serializerOptionsFactory.GetOptions();
    }

    public async Task<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken)
    {
        var retv = await JsonSerializer.DeserializeAsync<T>(stream, _serializerOptions, cancellationToken: cancellationToken);
        return retv;
    }

    public async Task<Stream?> Serialize(object? o)
    {
        if (o == null)
            return null;

        var retv = new MemoryStream();
        await JsonSerializer.SerializeAsync(retv, o, _serializerOptions);
        retv.Position = 0;
        return retv;
    }
}