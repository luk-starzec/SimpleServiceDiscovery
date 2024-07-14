using Discovery.Service.Interfaces;

namespace Discovery.Service.Services;

public class WatchDogService : IWatchDogService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISimpleDiscoveryService _simpleDiscoveryService;
    private readonly ILogger<IWatchDogService> _logger;

    public WatchDogService(IHttpClientFactory httpClientFactory, ISimpleDiscoveryService simpleDiscoveryService, ILogger<IWatchDogService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _simpleDiscoveryService = simpleDiscoveryService;
        _logger = logger;
    }

    public async Task StatusCheckAsync()
    {
        _logger.LogInformation($"Status check started at {DateTime.Now}");

        var httpClient = _httpClientFactory.CreateClient();

        var registeredServices = _simpleDiscoveryService.GetServices();

        _logger.LogInformation($"Found {registeredServices.Count()} services, {registeredServices.SelectMany(r => r.Value).Count()} endpoints");

        var invalidServices = new List<(string name, string url)>();
        foreach (var service in registeredServices)
        {
            _logger.LogInformation($"Checking {service.Key}");

            foreach (var url in service.Value)
            {
                bool isHealthy;
                try
                {
                    using var response
                        = await httpClient.GetAsync($"{url}/hc");

                    isHealthy = response.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    isHealthy = false;
                }

                if (!isHealthy)
                    invalidServices.Add((service.Key, url));

                _logger.LogInformation($"{service.Key} at {url} is {(isHealthy ? "healthy" : "unhealthy")}");
            }
        }

        foreach (var service in invalidServices)
        {
            _logger.LogInformation($"Removing {service.name} endpoint {service.url}");

            _simpleDiscoveryService.RemoveService(service.name, service.url);
        }

        _logger.LogInformation("Status check ended");
    }
}
