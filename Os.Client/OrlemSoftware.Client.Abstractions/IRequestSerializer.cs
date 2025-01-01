namespace OrlemSoftware.Client.Abstractions;

public interface IRequestSerializer<TConfiguration>: IRequestSerializer
    where TConfiguration : IApiClientConfiguration
{

}

public interface IRequestSerializer
{
    string ContentType { get; }
    Task<Stream?> Serialize(object? o);
}