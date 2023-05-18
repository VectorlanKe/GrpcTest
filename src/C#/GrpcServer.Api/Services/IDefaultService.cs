using Grpc.Core;

namespace GrpcServer.Api.Services
{
    public interface IDefaultService
    {
        Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context);
    }
}
