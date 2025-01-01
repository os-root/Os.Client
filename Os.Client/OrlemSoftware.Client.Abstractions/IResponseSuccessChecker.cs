namespace OrlemSoftware.Client.Abstractions;

public interface IResponseSuccessChecker<TConfiguration> : IResponseSuccessChecker
    where TConfiguration : IApiClientConfiguration
{
}

public interface IResponseSuccessChecker
{
    Task<bool> CheckResponseIsSuccessful(HttpResponseMessage response);
}