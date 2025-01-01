namespace OrlemSoftware.Client.Abstractions;

public interface IApiClientConfiguration
{
    string ApiBase { get; }
    TimeSpan? Timeout { get; }
    IReadOnlyDictionary<string, string> DefaultHeaders { get; }
}