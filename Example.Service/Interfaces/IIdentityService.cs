namespace Example.Service.Interfaces;

public interface IIdentityService
{
    string Name { get; set; }
    string GetAddress();
    string[] GetAddresses();
}
