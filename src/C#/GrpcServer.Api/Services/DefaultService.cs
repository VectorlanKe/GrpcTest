
using Grpc.Core;

namespace GrpcServer.Api.Services
{
    public class DefaultService: Default.DefaultBase, IDefaultService
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
