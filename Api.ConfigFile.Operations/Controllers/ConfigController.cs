using Api.ConfigFile.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.ConfigFile.Operations.Controllers
{
    public class ConfigController : Controller
    {
        private readonly ISettingsService _settingsService;

        public ConfigController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("api/servers/{serverName}/configs")]
        public IActionResult GetConfigSettings([FromRoute] string serverName)
        {
            var result = _settingsService.GetConfigSettings(serverName);
            return Ok(result);
        }

        [HttpGet("api/servers")]
        public IActionResult GetServerNames()
        {
            var result = _settingsService.GetServerNames();
            return Ok(result);
        }
        
        [HttpPatch("api/servers/{serverName}/configs")]
        public IActionResult UpdateConfigSettings([FromRoute] string serverName, [FromBody] Dictionary<string, string> updatedConfig)
        {
            _settingsService.UpdateConfigSettings(serverName, updatedConfig);
            return Ok();
        }
        
        [HttpPost("api/servers/{serverName}/configs")]
        public IActionResult AddConfigSettings([FromRoute] string serverName, [FromBody] Dictionary<string, string> newConfig)
        {
            _settingsService.AddConfigSettings(serverName, newConfig);
            return Ok();
        }
    }
}