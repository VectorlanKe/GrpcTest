using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using Thrift.Transport.Client;
using Thrift.Transport.Server;
using ThriftServer.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Default.IAsync, DefaultService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

TServerTransport serverSocketTransport = new TServerSocketTransport(7002, null);
var protocolFactory = new TBinaryProtocol.Factory();
TServer server = new TSimpleAsyncServer(new Default.AsyncProcessor(new DefaultService()), serverSocketTransport, protocolFactory,
    protocolFactory, LoggerFactory.Create(p => { p.AddConsole(); }));
server.ServeAsync(new CancellationToken());

app.Run();