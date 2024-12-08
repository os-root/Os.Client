
using Os.Client.Demo.ApiClient.Config;
using Os.Client.Di.Microsoft;
using Os.Client.Logging.Microsoft;

namespace Os.Client.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var apiClientConfig = new SelfClientConfig
            {
                ApiBase = "http://localhost:5037"
            };

            builder.Services.AddLogging(c =>
            {
                c.AddJsonConsole(x => x.IncludeScopes = true);
            });

            builder.Services
                .AddApiClient(apiClientConfig)
                .AddMsLogger()
                .AddHttpLogging(LogLevel.Information);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
