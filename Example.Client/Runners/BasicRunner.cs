using Grpc.Net.Client;

namespace Example.Client.Runners;

internal class BasicRunner : RunnerBase
{
    protected override string Name => nameof(BasicRunner);

    protected override GrpcChannel GetChannel()
    {
        return GrpcChannel.ForAddress("https://localhost:7001");
    }
}
