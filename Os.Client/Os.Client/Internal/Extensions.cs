namespace OrlemSoftware.Client.Internal;

internal static class Extensions
{
    public static void AddHttpHeaders(this HttpRequestMessage message, ApiClientRequest request)
    {
        foreach (var header in request.GetHeaders())
            message.Headers.Add(header.Key, header.Value);
    }
}