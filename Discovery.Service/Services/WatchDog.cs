using Discovery.Service.Interfaces;

namespace Discovery.Service.Services;

public class WatchDog : IWatchDog
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISimpleDiscovery _simpleDiscovery;
    private readonly ILogger<IWatchDog> _logger;

    public WatchDog(IHttpClientFactory httpClientFactory, ISimpleDiscovery simpleDiscovery, ILogger<IWatchDog> logger)
    {
        _httpClientFactory = httpClientFactory;
        _simpleDiscovery = simpleDiscovery;
        _logger = logger;
    }

    public async Task StatusCheckAsync()
    {
        _logger.LogInformation($"Status check started at {DateTime.Now}");

        var httpClient = _httpClientFactory.CreateClient();

        var registeredServices = _simpleDiscovery.GetServices();

        _logger.LogInformation($"Found {registeredServices.Count()} services");

        var invalidServices = new List<(string name, string url)>();
        foreach (var service in registeredServices)
        {
            _logger.LogInformation($"Checking {service.Key}");

            foreach (var url in service.Value)
            {
                using var response
                    = await httpClient.GetAsync($"{url}/hc");

                var isHealthy = response.IsSuccessStatusCode;

                if (!isHealthy)
                    invalidServices.Add((service.Key, url));

                _logger.LogInformation($"{service.Key}, {url} is {(isHealthy ? "healthy" : "unhealthy")}");
            }
        }

        foreach (var service in invalidServices)
        {
            _logger.LogInformation($"Removing {service.name} endpoint {service.url}");

            _simpleDiscovery.RemoveService(service.name, service.url);
        }

        _logger.LogInformation("Status check ended");
    }
}
