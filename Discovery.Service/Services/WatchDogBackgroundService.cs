using Discovery.Service.Interfaces;

namespace Discovery.Service.Services;

public class WatchDogBackgroundService : BackgroundService
{
    private readonly int _checkInterval;
    private readonly IWatchDogService _watchDogService;
    private readonly IConfiguration _configuration;

    public WatchDogBackgroundService(IWatchDogService watchDogService, IConfiguration configuration)
    {
        _watchDogService = watchDogService;
        _configuration = configuration;

        var interval = _configuration.GetValue<int?>("WatchDogIntervalSeconds") ?? 10;
        _checkInterval = interval * 1000;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _watchDogService.StatusCheckAsync();
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
