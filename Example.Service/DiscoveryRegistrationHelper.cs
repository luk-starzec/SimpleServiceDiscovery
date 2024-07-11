using Example.Shared;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Text;
using System.Text.Json;

namespace Example.Service;

public class DiscoveryRegistrationHelper
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServer _server;

    private readonly string _discoveryUrl;

    public DiscoveryRegistrationHelper(IConfiguration configuration, IHttpClientFactory httpClientFactory, IServer server)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _server = server;

        _discoveryUrl = _configuration.GetValue<string>("DiscoveryServiceUrl") ?? throw new Exception("DiscoveryServiceUrl is undefined");
    }
    public async Task RegisterInServiceDiscoveryAsync()
    {
        var addressFeature = _server.Features.Get<IServerAddressesFeature>();

        if (addressFeature is null)
            return;

        foreach (var address in addressFeature.Addresses)
        {
            await Register("Example.Service", address.Replace("0.0.0.0", "localhost"));
        }
    }

    private async Task Register(string name, string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(_discoveryUrl);

        try
        {
            var serviceUrl = new ServiceUrlDto
            {
                Name = name,
                Url = url
            };
            var content = new StringContent(JsonSerializer.Serialize(serviceUrl), Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync("api/Discovery", content);
        }
        catch (Exception)
        {
            // ignore
        }
    }
}
