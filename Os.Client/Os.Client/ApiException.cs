namespace OrlemSoftware.Client;

public class ApiException : Exception
{
    public int StatusCode { get; internal init; }
    public string? RawResponse { get; internal init; }

    public ApiException()
    {
        
    }

    public ApiException(Exception e)
        : base(e.Message, e)
    {
        
    }
}