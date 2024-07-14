namespace Discovery.Service.Interfaces;

public interface ISimpleDiscoveryService
{
    void RegisterService(string name, string url);
    void RemoveService(string name, string url);
    KeyValuePair<string, string[]>[] GetServices();
    string[] GetServiceUrls(string name);
}
