using Grpc.Core;
using GrpcServer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefaultController : ControllerBase
{
    private readonly IDefaultService _defaultService;

    public DefaultController(IDefaultService defaultService)
    {
        _defaultService = defaultService;
    }

    [HttpGet]
    public async ValueTask<string> GetAsync()
    {
        HelloReply result = await _defaultService.SayHello(new HelloRequest
        {
            Name = "123"
        },null);
        return result.Message;
    }
}