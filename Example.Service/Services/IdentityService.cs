using Example.Service.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Example.Service.Services;

public class IdentityService : IIdentityService
{
    private readonly IServer _server;

    public string Name { get; set; } = "Example.Service";

    public IdentityService(IServer server)
    {
        _server = server;
    }

    public string GetAddress()
    {
        return GetAddresses().FirstOrDefault() ?? "unknown";
    }

    public string[] GetAddresses()
    {
        var addressFeature = _server.Features.Get<IServerAddressesFeature>();
        var addresses = addressFeature?.Addresses.Select(NormalizeAddress).ToArray();
        return addresses ?? [];
    }


    private string NormalizeAddress(string address) =>
        address
            .Replace("0.0.0.0", "localhost")
            .Replace("127.0.0.1", "localhost");

}
