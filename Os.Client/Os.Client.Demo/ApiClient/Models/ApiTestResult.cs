using System.Text.Json.Serialization;

namespace OrlemSoftware.Client.Demo.ApiClient.Models;

internal record ApiTestResult
{
    [JsonPropertyName("state")]
    public string MyState { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreationDate { get; init; }
}