namespace OrlemSoftware.Client.Interfaces;

public interface IResponseDeserializer<TConfiguration>: IResponseDeserializer
    where TConfiguration : IApiClientConfiguration
{
}

public interface IResponseDeserializer
{
    Task<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken);
}