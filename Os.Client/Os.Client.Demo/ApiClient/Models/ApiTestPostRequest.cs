namespace Os.Client.Demo.ApiClient.Models;

internal record ApiTestPostRequest : BaseApiRequest
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