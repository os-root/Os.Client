using System.Net.Mime;
using System.Text.Json;
using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client;

public class DefaultClientJson<TConfiguration> : IRequestSerializer<TConfiguration>, IResponseDeserializer<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public static DefaultClientJson<TConfiguration> Instance { get; } = new();
    public string ContentType => MediaTypeNames.Application.Json;

    private DefaultClientJson()
    {
        
    }

    public async Task<Stream?> Serialize(object? o)
    {
        if (o == null)
            return null;

        var retv = new MemoryStream();
        await JsonSerializer.SerializeAsync(retv, o);
        retv.Position = 0;
        return retv;
    }

    public Task<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken) 
        => JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken).AsTask();
}