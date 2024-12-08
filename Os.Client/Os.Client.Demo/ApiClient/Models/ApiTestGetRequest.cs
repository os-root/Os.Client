namespace Os.Client.Demo.ApiClient.Models;

internal record ApiTestGetRequest : BaseApiRequest<ApiTestResult>
{
    public ApiTestGetRequest(string state)
        : base($"some?state={state}", HttpMethod.Get)
    {

    }
}