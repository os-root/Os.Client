namespace Os.Client.Interfaces;

public interface IRequestSerializer<TConfiguration>: IRequestSerializer
    where TConfiguration : IApiClientConfiguration
{

}

public interface IRequestSerializer
{
    string ContentType { get; }
    Task<Stream?> Serialize(object? o);
}