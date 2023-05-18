## 1、nuget包

Grpc.AspNetCore

## 2、添加.proto

## 3、服务端

Program.cs 

```csharp
...
builder.Services.AddGrpc();
...
app.MapGrpcService<DefaultService>();
...
```

## 4、客户端

Program.cs 

```csharp
...
var channel = GrpcChannel.ForAddress("https://localhost:7001");
Default.DefaultClient client = new Default.DefaultClient(channel);
builder.Services.AddSingleton(client);
...
```

