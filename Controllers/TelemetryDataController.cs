using IOTDeviceManagement.Models;
using IOTDeviceManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices.Client;

namespace IOTDeviceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryDataController : ControllerBase
    {
        private readonly TelemetryDataRepository _repository;
        public TelemetryDataController()
        {
            _repository = new TelemetryDataRepository();
        }
        [HttpPost]
        public async Task<IActionResult>PostTelemetryData( string deviceId,TelemetryData telemetryData)
        {
          try
            {
                var result = await _repository.SendTelemetryData(deviceId, telemetryData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
