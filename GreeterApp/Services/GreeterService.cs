using Grpc.Core;

namespace GreeterApp.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override Task<ByeReply> SayBye(ByeRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ByeReply{
            Message = "Bye Bye " + request.Name + " !!!"
        });
    }
}
