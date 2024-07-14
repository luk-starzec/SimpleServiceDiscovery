using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Client.Runners;

internal class StaticResolverRunner : RunnerBase
{
    protected override string Name => nameof(StaticResolverRunner);

    protected override GrpcChannel GetChannel()
    {
        var factory = new StaticResolverFactory(address => new[]
            {
                new BalancerAddress("localhost", 7001),
                new BalancerAddress("localhost", 7002),
            });

        var services = new ServiceCollection();
        services.AddSingleton<ResolverFactory>(factory);

        var channel = GrpcChannel.ForAddress(
            "static:///my-example-host",
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.SecureSsl,
                ServiceProvider = services.BuildServiceProvider(),
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
            });

        return channel;
    }
}
