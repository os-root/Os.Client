using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Di.Microsoft;
using OrlemSoftware.Client.Interfaces;

namespace OrlemSoftware.Client.Serialization.Text.Json.Di.Microsoft
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
