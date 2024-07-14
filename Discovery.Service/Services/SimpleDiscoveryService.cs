
using Discovery.Service.Interfaces;

namespace Discovery.Service.Services;

public class SimpleDiscoveryService : ISimpleDiscoveryService
{
    private readonly Dictionary<string, List<string>> _registeredServices = [];

    public KeyValuePair<string, string[]>[] GetServices()
    {
        return _registeredServices
            .Select(r => new KeyValuePair<string, string[]>(r.Key, r.Value.ToArray()))
            .ToArray();
    }

    public string[] GetServiceUrls(string name)
    {
        if (!_registeredServices.ContainsKey(name))
            return [];

        return [.. _registeredServices[name]];
    }

    public void RegisterService(string name, string url)
    {
        if (!_registeredServices.ContainsKey(name))
            _registeredServices.Add(name, []);

        _registeredServices[name].Add(url);
        _registeredServices[name] = _registeredServices[name]
            .Distinct()
            .ToList();
    }

    public void RemoveService(string name, string url)
    {
        if (!_registeredServices.ContainsKey(name))
            return;

        _registeredServices[name].Remove(url);
    }
}
