using System;
using System.Threading.Tasks;
using Greeter;
using Grpc.Core;

namespace GreeterServer
{
    class Program
    {
        class GreeterImpl : Greeter.Greeter.GreeterBase
        {
            // Server side handler of the SayHello RPC
            public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
            {
                return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
            }
        }
        static void Main(string[] args)
        {
            const int Port = 50051;
            Server server = new Server
            {
                Services = { Greeter.Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
