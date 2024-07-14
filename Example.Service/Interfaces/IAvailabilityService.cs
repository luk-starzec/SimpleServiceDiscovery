namespace Example.Service.Interfaces;

public interface IAvailabilityService
{
    void SetAvailability(bool enabled);
    bool GetAvailability();
}
