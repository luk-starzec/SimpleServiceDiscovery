using Example.Client.Utils;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Client.Runners;

internal class DiscoveryResolverRunner : RunnerBase
{
    protected override string Name => nameof(DiscoveryResolverRunner);

    protected override GrpcChannel GetChannel()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();
        services.AddSingleton<ResolverFactory, DiscoveryResolverFactory>();

        var channel = GrpcChannel.ForAddress(
            "discovery:///https://localhost:7801/api/discovery/Example.Service",
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.SecureSsl,
                ServiceProvider = services.BuildServiceProvider(),
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
            });

        return channel;
    }
}
