using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefaultController : ControllerBase
{
    private readonly Default.IAsync _defaultService;

    public DefaultController(Default.IAsync defaultService)
    {
        _defaultService = defaultService;
    }

    [HttpGet]
    public async ValueTask<string> GetAsync(CancellationToken cancellationToken = default)
    {
        string result = await _defaultService.SayHello("server ", cancellationToken);
        return result;
    }
}