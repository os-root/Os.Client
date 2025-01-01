using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using OrlemSoftware.Client.Abstractions;

namespace OrlemSoftware.Client;

public abstract record ApiClientRequest: IApiClientRequest
{
    private readonly string _urlTemplate;
    private readonly HttpMethod _method;
    private readonly IReadOnlyCollection<KeyValuePair<string, string>> _headers;
    private static readonly Regex StringParamRegex = new(@"{([^}]+)}");

    protected object? Body { get; set; }

    internal IRequestSerializer? InternalSerializer { get; set; }

    protected IRequestSerializer? Serializer
    {
        get => InternalSerializer;
        set => InternalSerializer = value;
    }

    protected ApiClientRequest(
            string urlTemplate,
            HttpMethod method,
            IReadOnlyCollection<KeyValuePair<string, string>> headers
        )
    {
        _urlTemplate = urlTemplate;
        _method = method;
        _headers = headers;
    }

    protected ApiClientRequest(
            string urlTemplate,
            HttpMethod method
        ) : this(urlTemplate, method, Array.Empty<KeyValuePair<string, string>>())
    {

    }

    internal string GetUrl()
    {
        if (!_urlTemplate.Contains("{")
            && !_urlTemplate.Contains("}"))
            return _urlTemplate;

        var paramNames = StringParamRegex.Matches(_urlTemplate)
            .Select(x => x.Groups[1].Value)
            .ToArray();

        var routeParamProperties = new List<(PropertyInfo PropertyInfo, string ParameterName)>();
        var queryParamProperties = new List<(PropertyInfo PropertyInfo, string ParameterName)>();

        foreach (var prop in GetType().GetProperties())
        {
            var routeParameterName = paramNames.FirstOrDefault(paramName =>
                paramName.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)
                || paramName.Equals(prop.Name.Replace("@", string.Empty), StringComparison.InvariantCultureIgnoreCase)
                || paramName.Equals(prop.GetCustomAttribute<RouteParameterAttribute>()?.Name,
                    StringComparison.InvariantCultureIgnoreCase)
            );

            var queryParameterName = routeParameterName != null
                ? null
                : prop.GetCustomAttribute<QueryParameterAttribute>()?.Name ?? prop.Name;

            if (routeParameterName != null)
                routeParamProperties.Add(new(prop, routeParameterName));

            if (queryParameterName != null)
                queryParamProperties.Add(new(prop, queryParameterName));
        }

        var retv = routeParamProperties
            .Where(paramProperty => !string.IsNullOrWhiteSpace(paramProperty.ParameterName))
            .Aggregate(new StringBuilder(_urlTemplate), (current, paramProperty) =>
                    current.Replace($"{{{paramProperty.ParameterName}}}", paramProperty.PropertyInfo.GetValue(this)?.ToString() ?? string.Empty)
                );

        if (queryParamProperties.Any())
            retv.Append('?');

        foreach (var queryParamProperty in queryParamProperties)
        {
            var value = queryParamProperty.PropertyInfo.GetValue(this)?.ToString();
            if (string.IsNullOrWhiteSpace(value))
                continue;

            retv.Append($"{queryParamProperty.ParameterName}={value}&");
        }

        return retv.ToString().TrimEnd('&');
    }

    internal HttpMethod GetMethod() => _method;
    internal IReadOnlyCollection<KeyValuePair<string, string>> GetHeaders() => _headers;
    internal Task<Stream?> GetBodyStream() => InternalSerializer.Serialize(Body);
    internal string GetContentType() => InternalSerializer.ContentType;
}

public abstract record ApiClientRequest<TResponse> : ApiClientRequest, IApiClientRequest<TResponse>
{
    protected ApiClientRequest(string urlTemplate, HttpMethod method, IReadOnlyCollection<KeyValuePair<string, string>> headers)
        : base(urlTemplate, method, headers)
    {
    }

    protected ApiClientRequest(string urlTemplate, HttpMethod method)
        : base(urlTemplate, method)
    {
    }
}