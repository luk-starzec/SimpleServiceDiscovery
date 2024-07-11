namespace Example.Shared;

public record ServiceUrlDto
{
    public required string Name { get; init; }
    public required string Url { get; init; }
}
