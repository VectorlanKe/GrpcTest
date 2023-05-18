using Microsoft.AspNetCore.Mvc;

namespace GrpcClient.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefaultController : ControllerBase
{
    private readonly Default.DefaultClient _defaultClient;

    public DefaultController(Default.DefaultClient defaultClient)
    {
        _defaultClient = defaultClient;
    }

    [HttpGet]
    public async ValueTask<string> GetAsync()
    {
        var result = await _defaultClient.SayHelloAsync(new HelloRequest
        {
            Name="client"
        });
        return result.Message;
    }
}