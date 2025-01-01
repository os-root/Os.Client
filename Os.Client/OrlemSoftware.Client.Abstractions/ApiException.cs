namespace OrlemSoftware.Client.Abstractions;

public class ApiException : Exception
{
    public int StatusCode { get;  }
    public string? RawResponse { get;   }

    public ApiException(int statusCode, string? rawResponse)
    {
        StatusCode = statusCode;
        RawResponse = rawResponse;
    }

    public ApiException(Exception e, int statusCode, string? rawResponse)
        : base(e.Message, e)
    {
        StatusCode = statusCode;
        RawResponse = rawResponse;
    }
}