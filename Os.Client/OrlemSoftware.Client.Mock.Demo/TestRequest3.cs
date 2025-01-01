using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Demo;

internal record TestRequest3 : IApiClientRequest
{
    public string State { get; init; }
}