using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Grpc.Core;
using System.Text.Json;
using System.Net.Http.Json;

namespace Example.Client.Utils;

public class DiscoveryResolver : PollingResolver
{
    private readonly string _dicsoveryAddress;
    private readonly TimeSpan _refreshInterval;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    private Timer? _timer;

    public DiscoveryResolver(string dicsoveryAddress, TimeSpan refreshInterval, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _dicsoveryAddress = dicsoveryAddress;
        _refreshInterval = refreshInterval;
        _httpClientFactory = httpClientFactory;
        _logger = loggerFactory.CreateLogger<DiscoveryResolver>();
    }

    protected override void OnStarted()
    {
        base.OnStarted();

        _timer = new Timer(OnTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
        _timer.Change(_refreshInterval, _refreshInterval);
    }

    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("DiscoveryResolver: Resolve");

            var httpClient = _httpClientFactory.CreateClient();
            var addresses = await httpClient.GetFromJsonAsync<string[]>(_dicsoveryAddress);

            var balancerAddresses = addresses
                .Select(r => new Uri(r))
                .Select(r => new BalancerAddress(r.Host, r.Port)).ToList();

            Console.WriteLine($"DiscoveryResolver: Resolved {balancerAddresses.Count} endpoints");

            Listener(ResolverResult.ForResult(balancerAddresses));
        }
        catch (Exception ex)
        {
            Listener(ResolverResult.ForFailure(new Status(StatusCode.Unavailable, "Service discovery error", ex)));
        }

    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _timer?.Dispose();
    }

    private void OnTimerCallback(object? state)
    {
        try
        {
            Console.WriteLine("DiscoveryResolver: Timer tick");
            Refresh();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refresh interval error");
        }
    }
}

public class DiscoveryResolverFactory : ResolverFactory
{
    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(15);
    private readonly IHttpClientFactory _httpClientFactory;

    public DiscoveryResolverFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override string Name => "discovery";

    public override Resolver Create(ResolverOptions options)
    {
        return new DiscoveryResolver(options.Address.LocalPath.TrimStart('/'), _refreshInterval, _httpClientFactory, options.LoggerFactory);
    }
}
