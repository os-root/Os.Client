namespace Os.Client.Internal;

internal static class Extensions
{
    public static void AddHttpHeaders(this HttpRequestMessage message, BaseApiRequest request)
    {
        foreach (var header in request.GetHeaders())
            message.Headers.Add(header.Key, header.Value);
    }
}