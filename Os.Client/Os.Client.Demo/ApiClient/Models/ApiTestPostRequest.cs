namespace OrlemSoftware.Client.Demo.ApiClient.Models;

internal record ApiTestPostRequest : ApiClientRequest
{
    public ApiTestPostRequest(string state)
        : base($"some", HttpMethod.Post)
    {
        Body = new
        {
            state,
            createdAt = DateTimeOffset.UtcNow,
        };
    }
}