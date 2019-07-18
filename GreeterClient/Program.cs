using GrpcServer;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreeterClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();

            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new HelloServer.HelloServerClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "you" });
            Console.WriteLine("Greeting: " + reply.Message);
            //服务端流
            using (var call = client.SayHelloOneStremOne(new HelloRequest { Name = "Stream you" }))
            {
                while (call.ResponseStream.MoveNext().Result)
                {
                    HelloReply feature = call.ResponseStream.Current;
                    Console.WriteLine("服务端流:" + feature.Message);
                }
            }
            //客户端流
            using (var call = client.SayHelloOneStremTwo())
            {
                foreach (var item in Enumerable.Range(0, 10))
                {
                    call.RequestStream.WriteAsync(new HelloRequest() { Name = "client msg" + item }).Wait();
                }
                call.RequestStream.CompleteAsync();

                HelloReply summary = call.ResponseAsync.Result;
                Console.WriteLine("客户端流:" + summary.Message);
            }
            //双向流
            using (var call = client.SayHelloTwoStrem())
            {
                var responseReaderTask = Task.Run(() =>
                {
                    while (call.ResponseStream.MoveNext().Result)
                    {
                        var note = call.ResponseStream.Current;
                        Console.WriteLine("双向： " + note);
                    }
                });

                foreach (var item in Enumerable.Range(0, 10))
                {
                    call.RequestStream.WriteAsync(new HelloRequest() { Name = "client msg" + item }).Wait();
                }
                call.RequestStream.CompleteAsync();
                responseReaderTask.Wait();
            }
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
