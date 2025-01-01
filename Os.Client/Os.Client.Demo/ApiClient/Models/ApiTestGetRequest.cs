namespace OrlemSoftware.Client.Demo.ApiClient.Models;

internal record ApiTestGetRequest : ApiClientRequest<ApiTestResult>
{
    public ApiTestGetRequest(string state)
        : base($"some?state={state}", HttpMethod.Get)
    {

    }
}