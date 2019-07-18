using System;
using System.Threading.Tasks;
using GrpcServer;
using Grpc.Core;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GreeterServer
{
    class Program
    {
        class GreeterImpl : HelloServer.HelloServerBase
        {
            // Server side handler of the SayHello RPC
            public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
            {
                return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
            }
            /// <summary>
            /// 服务端流
            /// </summary>
            /// <param name="request"></param>
            /// <param name="responseStream"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public override async Task SayHelloOneStremOne(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
            {
                foreach (var item in Enumerable.Range(0, 10))
                {
                    await responseStream.WriteAsync(new HelloReply() { Message = "serverStream:" + request.Name+ item });
                }
            }
            /// <summary>
            /// 客户端流
            /// </summary>
            /// <param name="requestStream"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public override async Task<HelloReply> SayHelloOneStremTwoAsync(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
            {
                var stopwatch = new Stopwatch();
                StringBuilder stringBuilder = new StringBuilder();
                stopwatch.Start();
                while (await requestStream.MoveNext())
                {
                    var helloReply = requestStream.Current;
                    stringBuilder.Append("clientStream:"+helloReply.Name);
                }

                stopwatch.Stop();

                return new HelloReply
                {
                    Message= stringBuilder.ToString()
                };
            }
            /// <summary>
            /// 双向流
            /// </summary>
            /// <param name="requestStream"></param>
            /// <param name="responseStream"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public override async Task SayHelloTwoStrem(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
            {
                while (await requestStream.MoveNext())
                {
                    var request = requestStream.Current;
                    await responseStream.WriteAsync(new HelloReply() {
                        Message="way-way Stream:"+ request.Name
                    });
                }
            }
        }
        static void Main(string[] args)
        {
            const int Port = 50051;
            Server server = new Server
            {
                Services = { HelloServer.BindService(new GreeterImpl()) },
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
