using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client.Generic;

public class GenericResponseSuccessChecker<TConfiguration> : IResponseSuccessChecker<TConfiguration>
    where TConfiguration : IApiClientConfiguration
{
    public GenericResponseSuccessChecker()
    {

    }

    public Task<bool> CheckResponseIsSuccessful(HttpResponseMessage response)
        => Task.FromResult(response.IsSuccessStatusCode);
}