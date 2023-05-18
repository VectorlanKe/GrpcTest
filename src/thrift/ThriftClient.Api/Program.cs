using Microsoft.Extensions.DependencyInjection;
using Thrift.Protocol;
using Thrift.Transport.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var socketTransport = new TSocketTransport("localhost", 7002, null);
TProtocol protocol = new TBinaryProtocol(socketTransport);
builder.Services.AddSingleton<Default.IAsync,Default.Client>(p => new Default.Client(protocol));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
