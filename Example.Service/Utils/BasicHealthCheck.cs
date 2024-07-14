using Example.Service.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Example.Service.Utils;

public class BasicHealthCheck : IHealthCheck
{
    private readonly IAvailabilityService _availabilityService;

    public BasicHealthCheck(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return _availabilityService.GetAvailability()
            ? Task.FromResult(HealthCheckResult.Healthy())
            : Task.FromResult(HealthCheckResult.Unhealthy());
    }
}
