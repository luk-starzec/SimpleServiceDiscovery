using Discovery.Service.Interfaces;
using Example.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Discovery.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        private readonly ISimpleDiscovery simpleDiscovery;

        public DiscoveryController(ISimpleDiscovery simpleDiscovery)
        {
            this.simpleDiscovery = simpleDiscovery;
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string[]>> Get()
        {
            return simpleDiscovery.GetServices();
        }

        [HttpGet("{serviceName}")]
        public string[] Get(string serviceName)
        {
            return simpleDiscovery.GetServiceUrls(serviceName);
        }

        [HttpPost]
        public void Post([FromBody]ServiceUrlDto service)
        {
            simpleDiscovery.RegisterService(service.Name, service.Url);
        }

        [HttpDelete("{serviceName}/{serviceUrl}")]
        public void Delete(string serviceName, string serviceUrl)
        {
            simpleDiscovery.RemoveService(serviceName, serviceUrl);
        }
    }
}
