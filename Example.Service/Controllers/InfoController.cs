using Example.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Example.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IDiscoveryRegistrationService _discoveryRegistrationService;
        private readonly IIdentityService _identityService;
        private readonly IAvailabilityService _availabilityService;

        public InfoController(IDiscoveryRegistrationService discoveryRegistrationService, IIdentityService identityService, IAvailabilityService availabilityService)
        {
            _discoveryRegistrationService = discoveryRegistrationService;
            _identityService = identityService;
            _availabilityService = availabilityService;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var name = _identityService.Name;
            var addresses = string.Join(", ", _identityService.GetAddresses());
            var isEnabled = _availabilityService.GetAvailability();

            return [name, addresses, isEnabled ? "enabled" : "disabled"];
        }

        [HttpPost("register")]
        public void Register()
        {
            _discoveryRegistrationService.RegisterInServiceDiscoveryAsync();
        }

        [HttpPost("enable")]
        public void Enable(bool notifyDiscovery)
        {
            _availabilityService.SetAvailability(true);
            if (notifyDiscovery)
                _discoveryRegistrationService.RegisterInServiceDiscoveryAsync();
        }

        [HttpPost("disable")]
        public void Disable(bool notifyDiscovery)
        {
            _availabilityService.SetAvailability(false);
            if (notifyDiscovery)
                _discoveryRegistrationService.RemoveFromServiceDiscoveryAsync();
        }
    }
}
