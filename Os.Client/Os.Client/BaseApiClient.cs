﻿using System.Net.Http.Headers;
using OrlemSoftware.Client.Abstractions;
using OrlemSoftware.Client.Internal;

namespace OrlemSoftware.Client;

public abstract class BaseApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IEnumerable<IApiClientProcessingStrategy> _processingStrategies;
    private readonly IResponseSuccessChecker _responseSuccessChecker;
    private readonly IResponseDeserializer _deserializer;
    private readonly IRequestSerializer _serializer;

    private readonly IClientLogger _logger;
    
    protected BaseApiClient(HttpClient httpClient,
        IEnumerable<IApiClientProcessingStrategy> processingStrategies,
        IResponseSuccessChecker responseSuccessChecker,
        IClientLoggerFactory loggerFactory,
        IResponseDeserializer deserializer,
        IRequestSerializer serializer)
    {
        _httpClient = httpClient;
        _processingStrategies = processingStrategies;
        _responseSuccessChecker = responseSuccessChecker;
        _deserializer = deserializer;
        _serializer = serializer;
        _logger = loggerFactory.CreateLogger(this);
    }

    public Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request)
        => Send(request, CancellationToken.None);

    public Task Send(IApiClientRequest request)
        => Send(request, CancellationToken.None);

    public async Task<TResponse> Send<TResponse>(IApiClientRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var response = await SendInternal(request, cancellationToken);

        try
        {
            if (!await CheckIfResponseSucceeded(response))
                throw new ApiException((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));

            var responseStr = await response.Content.ReadAsStreamAsync(cancellationToken);

            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.Log(ClientLogLevel.Debug, "RawResponse: {raw}", raw);

            responseStr.Position = 0;
            var retv = await _deserializer.Deserialize<TResponse>(responseStr, cancellationToken);
            return retv!;
        }
        catch (Exception e)
        {
            throw new ApiException(e, (int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    public async Task Send(IApiClientRequest request, CancellationToken cancellationToken)
    {
        var response = await SendInternal(request, cancellationToken);
        if (!await CheckIfResponseSucceeded(response))
            throw new ApiException((int) response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken));
    }

    private async Task<HttpResponseMessage> SendInternal(IApiClientRequest req, CancellationToken cancellationToken)
    {
        if (req is not ApiClientRequest request)
            throw new InvalidOperationException($"Request of type {req.GetType()} cannot be used with ApiClient {GetType()}. " +
                                                $"Request must be inherited from {typeof(ApiClientRequest)}");

        request.InternalSerializer ??= _serializer;
        var message = await MakeMessage(request);
        var response = await _httpClient.SendAsync(message, cancellationToken);

        if (await CheckIfResponseSucceeded(response))
            return response;

        var strategiesToApply = new List<IApiClientProcessingStrategy>();

        foreach (var processingStrategy in _processingStrategies)
        {
            if (!await processingStrategy.CheckCanApplyAsync(response))
                continue;
            strategiesToApply.Add(processingStrategy);
        }

        if (!strategiesToApply.Any())
            return response;

        message = await MakeMessage(request);//cannot send old message again
        foreach (var processingStrategy in strategiesToApply)
        {
            _logger.Log(ClientLogLevel.Debug, "Running Api Client Strategy: {strategyType}", processingStrategy.GetType());
            await processingStrategy.ApplyAsync(message);
        }

        response = await _httpClient.SendAsync(message, cancellationToken);
        return response;
    }

    protected virtual Task<bool> CheckIfResponseSucceeded(HttpResponseMessage response)
        => _responseSuccessChecker.CheckResponseIsSuccessful(response);

    private async Task<HttpRequestMessage> MakeMessage(ApiClientRequest request)
    {
        var absPath = _httpClient.BaseAddress?.AbsolutePath;
        var url = request.GetUrl();
        if (!string.IsNullOrWhiteSpace(absPath) && !absPath.Equals("/"))
            url = $"{absPath}{url}";

        var message = new HttpRequestMessage(request.GetMethod(), url);
        message.AddHttpHeaders(request);

        foreach (var processingStrategy in _processingStrategies)
        {
            if (await processingStrategy.CheckCanApplyAsync(message))
                await processingStrategy.ApplyAsync(message);
        }

        var bodyStream = await request.GetBodyStream();
        if (bodyStream != null)
        {
            message.Content = new StreamContent(bodyStream)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue(request.GetContentType()),
                    ContentLength = bodyStream.Length
                }
            };
        }

        return message;
    }
}