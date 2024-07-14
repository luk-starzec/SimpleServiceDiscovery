using Example.Service.Interfaces;

namespace Example.Service.Services;

public class AvailabilityService : IAvailabilityService
{
    private bool _enabled = true;

    public bool GetAvailability() => _enabled;

    public void SetAvailability(bool enabled) => _enabled = enabled;
}
