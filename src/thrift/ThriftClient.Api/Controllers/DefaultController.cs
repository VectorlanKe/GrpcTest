using Microsoft.AspNetCore.Mvc;
using Thrift.Protocol;
using Thrift.Transport.Client;

namespace ThriftClient.Api.Controllers;

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
        return await _defaultService.SayHello($"client {DateTime.Now:yyyy-MM-dd HH:mm:ss}", cancellationToken);
    }
}