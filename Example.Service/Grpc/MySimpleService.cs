using Example.Service.Interfaces;
using Example.Shared;
using MagicOnion;
using MagicOnion.Server;

namespace Example.Service.Grpc;

public class MySimpleService : ServiceBase<IMySimpleService>, IMySimpleService
{
    private readonly IIdentityService _identityService;

    public MySimpleService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public UnaryResult<string> PrintMessage(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] Message: {message}");

        var serviceAddress = _identityService.GetAddress();

        return UnaryResult.FromResult($"Printed at {serviceAddress}");
    }
}