using Discovery.Service.Interfaces;
using Example.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Discovery.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        private readonly ISimpleDiscoveryService _simpleDiscovery;
        private readonly ILogger<DiscoveryController> _logger;

        public DiscoveryController(ISimpleDiscoveryService simpleDiscovery, ILogger<DiscoveryController> logger)
        {
            _simpleDiscovery = simpleDiscovery;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string[]>> Get()
        {
            return _simpleDiscovery.GetServices();
        }

        [HttpGet("{serviceName}")]
        public string[] Get(string serviceName)
        {
            return _simpleDiscovery.GetServiceUrls(serviceName);
        }

        [HttpPost("add")]
        public void Add([FromBody]ServiceUrlDto service)
        {
            _logger.LogInformation($"Added {service.Name} with {service.Url}");

            _simpleDiscovery.RegisterService(service.Name, service.Url);
        }

        [HttpPost("remove")]
        public void Remove([FromBody] ServiceUrlDto service)
        {
            _logger.LogInformation($"Removed {service.Name} with {service.Url}");

            _simpleDiscovery.RemoveService(service.Name, service.Url);
        }

        [HttpPost("init")]
        public void InitDefault()
        {
            _logger.LogInformation($"Init");

            _simpleDiscovery.RegisterService("Example.Service", "https://localhost:7001");
            _simpleDiscovery.RegisterService("Example.Service", "https://localhost:7002");
        }
    }
}
