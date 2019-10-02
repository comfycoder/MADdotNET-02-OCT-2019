using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MonitorMyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ILogger<TestsController> _logger;
        private readonly TelemetryClient _telemetry;

        public TestsController(ILogger<TestsController> logger, TelemetryClient telemetry)
        {
            _logger = logger;
            _telemetry = telemetry;
        }

        [HttpGet("logger")]
        public ActionResult<string> LoggerMethods()
        {
            _logger.LogTrace("LOGGER: Write a 'trace' message");
            _logger.LogDebug("LOGGER: Write a 'debug' message");
            _logger.LogInformation("LOGGER: Write an 'info' message");
            _logger.LogWarning("LOGGER: Write a 'warning' message");
            _logger.LogError("LOGGER: Write an 'error' message");
            _logger.LogCritical("LOGGER: Write a 'critical' message");

            return "SUCCESS";
        }

        [HttpGet("telemetry")]
        public ActionResult<string> TelemetryMethods()
        {
            _telemetry.TrackEvent("Telemetry Event: Write a 'trace' message");
            _telemetry.TrackException(new ApplicationException("Telemetry Exception: An unknown exception occured."));

            return "SUCCESS";
        }

        [HttpGet("exception")]
        public ActionResult<string> UnhandledException()
        {
            throw new ApplicationException("Unhandled Exception: An unhandled exception occured.");
        }
    }
}