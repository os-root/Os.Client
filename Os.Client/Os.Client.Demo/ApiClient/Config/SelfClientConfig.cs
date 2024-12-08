using Os.Client.Interfaces;

namespace Os.Client.Demo.ApiClient.Config;

public record SelfClientConfig : IApiClientConfiguration
{
    public string ApiBase { get; set; }
    public TimeSpan? Timeout { get; set; }
    public IReadOnlyDictionary<string, string> DefaultHeaders { get; } = new Dictionary<string, string>();
}