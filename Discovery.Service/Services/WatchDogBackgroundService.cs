using Discovery.Service.Interfaces;

namespace Discovery.Service.Services;

public class WatchDogBackgroundService : BackgroundService
{
    private readonly int _checkInterval = 10000;
    private readonly IWatchDog _watchDog;

    public WatchDogBackgroundService(IWatchDog watchDog)
    {
        _watchDog = watchDog;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _watchDog.StatusCheckAsync();
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
