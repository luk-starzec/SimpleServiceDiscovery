using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Example.Client.Utils;

public class FileResolver : PollingResolver
{
    private readonly string _filePath;

    public FileResolver(string filePath, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _filePath = filePath;
    }

    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        var jsonString = await File.ReadAllTextAsync(_filePath);
        var results = JsonSerializer.Deserialize<int[]>(jsonString);
        var addresses = results.Select(r => new BalancerAddress("localhost", r)).ToArray();

        // Pass the results back to the channel.
        Listener(ResolverResult.ForResult(addresses));
    }
}

public class FileResolverFactory : ResolverFactory
{
    // Create a FileResolver when the URI has a 'file' scheme.
    public override string Name => "file";

    public override Resolver Create(ResolverOptions options)
    {
        var filePath = options.Address.LocalPath.TrimStart('/');
        return new FileResolver(filePath, options.LoggerFactory);
    }
}