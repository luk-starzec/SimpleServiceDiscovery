using Example.Client.Utils;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Client.Runners;

internal class FileResolverRandomBalanserRunner : RunnerBase
{
    protected override string Name => nameof(FileResolverRandomBalanserRunner);

    protected override GrpcChannel GetChannel()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ResolverFactory, FileResolverFactory>();
        services.AddSingleton<LoadBalancerFactory, RandomBalancerFactory>();

        var channel = GrpcChannel.ForAddress(
            "file:///data/ports.json",
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.SecureSsl,
                ServiceProvider = services.BuildServiceProvider(),
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new LoadBalancingConfig("random") } }
            });

        return channel;
    }
}
