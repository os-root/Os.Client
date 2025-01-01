using Microsoft.Extensions.DependencyInjection;
using OrlemSoftware.Client.Abstractions;
using OrlemSoftware.Client.Mock.Di.Microsoft;

namespace OrlemSoftware.Client.Mock.Demo;

internal class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMockApiClient<TestConfig>()
            .AddRequestHandler<TestRequest, TestResponse>(
                s => true,
                r => Task.FromResult(new TestResponse
                {
                    State = r.State,
                    Response = "I am test!"
                })
            );

        var sp = serviceCollection.BuildServiceProvider();

        var apiClient = sp.GetRequiredService<IApiClient<TestConfig>>();
        var response = await apiClient.Send(new TestRequest
        {
            State = "Test state from sending"
        });

        try
        {
            response = await apiClient.Send(new TestRequest2
            {
                State = "Test state from sending"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}