using Example.Shared;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace Example.Client.Runners;

internal abstract class RunnerBase
{
    public int Delay { get; set; } = 500;

    public int Iterations { get; set; } = 10;

    protected abstract string Name { get; }

    protected abstract GrpcChannel GetChannel();

    public async Task RunAsync()
    {
        var channel = GetChannel();

        for (int i = 0; i < Iterations; i++)
        {
            var client = MagicOnionClient.Create<IMySimpleService>(channel);

            var result = await client.PrintMessage($"{Name} #{i}");
            Console.WriteLine($"Result #{i}: {result}");

            await Task.Delay(Delay);
        }
    }
}
