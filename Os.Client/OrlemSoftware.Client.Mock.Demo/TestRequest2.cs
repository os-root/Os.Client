using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Mock.Demo;

internal record TestRequest2 : IApiClientRequest<TestResponse>
{
    public string State { get; init; }
}