namespace Os.Client.Serialization;

public abstract class ApiBodyConverter
{
    public string ContentType { get; }

    protected ApiBodyConverter(string contentType)
    {
        ContentType = contentType;
    }

    public abstract Task<Stream?> ConvertAsync(object? o);
}