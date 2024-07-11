using Example.Shared;
using Grpc.Net.Client;
using MagicOnion.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7101");

var client = MagicOnionClient.Create<IMyFirstService>(channel);

var result = await client.SumAsync(123, 456);

Console.WriteLine($"Result: {result}");

Console.ReadLine();