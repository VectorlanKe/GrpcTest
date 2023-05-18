namespace ThriftServer.Api.Services
{
    public class DefaultService: Default.IAsync
    {
        public Task<string> SayHello(string request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult($"hello thrift {request}");
        }
    }
}
