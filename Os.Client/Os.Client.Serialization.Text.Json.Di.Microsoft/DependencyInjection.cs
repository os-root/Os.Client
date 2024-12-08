using Os.Client.Di.Microsoft;
using Os.Client.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Os.Client.Serialization.Text.Json.Di.Microsoft
{
    public static class DependencyInjection
    {
        public static IApiClientBuilder<TConfiguration> AddMsJson<TConfiguration>(this IApiClientBuilder<TConfiguration> builder, Action<TConfiguration, JsonSerializerOptions>? serializerSetup = null)
            where TConfiguration : IApiClientConfiguration
        {
            builder.Services.AddSingleton(serializerSetup ?? NullSetup);

            builder.AddRequestSerializer<MsJson<TConfiguration>>();
            builder.AddResponseDeserializer<MsJson<TConfiguration>>();

            return builder;
            void NullSetup(TConfiguration configuration, JsonSerializerOptions jsonSerializerOptions) { }
        }
    }
}
