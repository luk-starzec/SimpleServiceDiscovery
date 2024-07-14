using Example.Service.Interfaces;
using Example.Shared;
using System.Text;
using System.Text.Json;

namespace Example.Service.Services;

public class DiscoveryRegistrationService : IDiscoveryRegistrationService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DiscoveryRegistrationService> _logger;
    private readonly IIdentityService _identityService;
    private readonly string _discoveryUrl;

    public DiscoveryRegistrationService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<DiscoveryRegistrationService> logger, IIdentityService identityService)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _identityService = identityService;

        _discoveryUrl = _configuration.GetValue<string>("DiscoveryServiceUrl") ?? throw new Exception("DiscoveryServiceUrl is undefined");
    }
    public async Task RegisterInServiceDiscoveryAsync()
    {
        var name = _identityService.Name;
        var addresses = _identityService.GetAddresses();

        foreach (var address in addresses)
        {
            await RegisterAsync(name, address);
        }
    }

    public async Task RemoveFromServiceDiscoveryAsync()
    {
        var name = _identityService.Name;
        var addresses = _identityService.GetAddresses();

        foreach (var address in addresses)
        {
            await RemoveAsync(name, address);
        }
    }

    private async Task RegisterAsync(string name, string url)
    {
        try
        {
            await CallMethodAsync("api/discovery/add", name, url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register in service discovery error");
        }
    }

    private async Task RemoveAsync(string name, string url)
    {
        try
        {
            await CallMethodAsync("api/discovery/remove", name, url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Remove from service discovery error");
        }
    }

    private async Task CallMethodAsync(string methodUrl, string name, string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(_discoveryUrl);

        var payload = new ServiceUrlDto
        {
            Name = name,
            Url = url
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(methodUrl, content);
    }
}
