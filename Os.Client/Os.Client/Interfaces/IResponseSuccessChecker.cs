namespace OrlemSoftware.Client.Interfaces;

public interface IResponseSuccessChecker<TConfiguration> : IResponseSuccessChecker
    where TConfiguration : IApiClientConfiguration
{
}

public interface IResponseSuccessChecker
{
    Task<bool> CheckResponseIsSuccessful(HttpResponseMessage response);
}