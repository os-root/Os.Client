using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client;

public abstract record ApiClientConfiguration : IApiClientConfiguration
{
    public string ApiBase { get; init; }
    public TimeSpan? Timeout { get; init; }
    public Dictionary<string, string> DefaultHeaders { get; init; } = new();
    IReadOnlyDictionary<string, string> IApiClientConfiguration.DefaultHeaders => DefaultHeaders;
}