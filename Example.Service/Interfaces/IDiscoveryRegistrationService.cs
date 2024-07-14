namespace Example.Service.Interfaces;

public interface IDiscoveryRegistrationService
{
    Task RegisterInServiceDiscoveryAsync();
    Task RemoveFromServiceDiscoveryAsync();
}
