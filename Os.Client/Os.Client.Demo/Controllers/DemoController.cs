using Microsoft.AspNetCore.Mvc;
using OrlemSoftware.Client.Abstractions;
using OrlemSoftware.Client.Demo.ApiClient.Config;
using OrlemSoftware.Client.Demo.ApiClient.Models;

namespace OrlemSoftware.Client.Demo.Controllers;

[ApiController]
[Route("[controller]")]

public class DemoController : ControllerBase
{
    private readonly IApiClient<SelfClientConfig> _apiClient;

    public DemoController(IApiClient<SelfClientConfig> apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet("api-get")]
    public async Task<IActionResult> TestApiGet([FromQuery] string state)
    {
        var req = new ApiTestGetRequest(state);
        var result = await _apiClient.Send(req);
        return Ok(result);
    }

    [HttpGet("api-post")]
    public async Task<IActionResult> TestApiPost([FromQuery] string state)
    {
        var req = new ApiTestPostRequest(state);
        await _apiClient.Send(req);
        return Ok();
    }
}