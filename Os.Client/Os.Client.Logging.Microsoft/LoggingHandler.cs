using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace OrlemSoftware.Client.Logging.Microsoft;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger _logger;
    private readonly LogLevel _logLevel;

    public LoggingHandler(ILogger<LoggingHandler> logger, HttpMessageHandler innerHandler, LoggingHandlerConfiguration configuration)
        : base(innerHandler)
    {
        _logger = logger;
        _logLevel = configuration.HttpLogsLevel;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = new Stopwatch();
        HttpResponseMessage? response = null;

        try
        {
            sw.Start();
            response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            return response;
        }
        finally
        {
            await Log(request, response, sw.Elapsed);
            ThrowTimeoutIfOccured(response);
        }
    }

    #region Private methods

    /// <summary>
    /// Checks for timeout
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="TimeoutException"></exception>
    private void ThrowTimeoutIfOccured(HttpResponseMessage? response)
    {
        if (response == null) return;
        if (response.StatusCode != 0) return;
        throw new TimeoutException();
    }

    /// <summary>
    /// Logs raw http request & response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async Task Log(HttpRequestMessage request, HttpResponseMessage? response, TimeSpan duration)
    {
        var sb = new StringBuilder();
        var uri = request.RequestUri;

        sb.AppendLine($"{request.Method} {uri}");

        foreach (var header in request.Headers)
            sb.AppendLine($"{header.Key}: {string.Join(',', header.Value)}");
        sb.AppendLine();

        sb.AppendLine((request.Content != null ? await request.Content.ReadAsStringAsync() : null));

        var serializedRequest = sb.ToString();

        sb.Clear();

        if (response != null)
        {
            sb.AppendLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.StatusCode}");
            foreach (var header in response.Headers)
                sb.AppendLine($"{header.Key}: {string.Join(',', header.Value)}");
            sb.AppendLine();

            sb.AppendLine(await response.Content.ReadAsStringAsync());
        }

        var serializedResponse = sb.ToString();

        using var scope = _logger.BeginScope(
            new Dictionary<string, object>
            {
                { "RawRequest", serializedRequest },
                { "RawResponse", serializedResponse }
            });

        _logger.Log(_logLevel, "Remote service {Url} responded in {Duration}ms.", uri, duration.TotalMilliseconds);
    }

    #endregion
}