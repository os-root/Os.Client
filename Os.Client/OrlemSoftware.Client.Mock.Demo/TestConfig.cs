using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Demo;

internal record TestConfig : IApiClientConfiguration
{
    public string ApiBase { get; set; }
    public TimeSpan? Timeout { get; set; }
    public IReadOnlyDictionary<string, string> DefaultHeaders { get; set; }
}