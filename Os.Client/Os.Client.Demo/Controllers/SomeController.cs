using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace OrlemSoftware.Client.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class SomeController : ControllerBase
{
    public record TestModel
    {
        [JsonPropertyName("state")]
        public string State { get; init; }
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; init; }
    }

    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(TestModel))]
    public IActionResult GetTestModel([FromQuery] string state)
    {
        if (string.IsNullOrWhiteSpace(state))
            return BadRequest("state is required");

        var result = new TestModel
        {
            State = state,
            CreatedAt = DateTimeOffset.Now,
        };

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(TestModel))]
    public IActionResult SetTestModel([FromBody] TestModel? model)
    {
        if (model == null)
            return BadRequest();

        return Ok(model.State);
    }
}