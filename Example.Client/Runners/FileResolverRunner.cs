using Example.Client.Utils;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Client.Runners;

internal class FileResolverRunner : RunnerBase
{
    protected override string Name => nameof(FileResolverRunner);

    protected override GrpcChannel GetChannel()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ResolverFactory, FileResolverFactory>();

        var channel = GrpcChannel.ForAddress(
            "file:///data/ports.json",
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.SecureSsl,
                ServiceProvider = services.BuildServiceProvider(),
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
            });

        return channel;
    }
}
