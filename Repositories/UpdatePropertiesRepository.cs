using IOTDeviceManagement.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace IOTDeviceManagement.Repositories
{
    public class UpdatePropertiesRepository
    {
        private static string connectionString = "HostName=pavitrahub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=je6fA4iZSVkd6NMdahtbZJKxz27y1n5vOSUFf3HFcVs=";
        public static async Task<bool> IsDeviceAvailable(string deviceId)
        {
            var registrymanager = RegistryManager.CreateFromConnectionString(connectionString);
            Device device = await registrymanager.GetDeviceAsync(deviceId);
            if (device.Status == DeviceStatus.Enabled)
            {
                return true;
            }
            return false;
        }
        public async Task<string> UpdateReportedPropertiesAsync(UpdateProperties properties , string deviceId)
        {
          var deviceClient = DeviceClient.CreateFromConnectionString(connectionString,deviceId);
          {
            if(await IsDeviceAvailable(deviceId)) 
            {
                    var reportedProperties = new TwinCollection();
                    reportedProperties[properties.Key] = properties.Value;
                    await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
                    return "Reported Properties updated successfully";
            }
            return "Device is disabled";
          }
        }
        public async Task<string> UpdateDesiredPropertiesAsync(UpdateProperties properties, string deviceId)
        {
            var registryManager = RegistryManager.CreateFromConnectionString(connectionString);
           {
                if (await IsDeviceAvailable(deviceId))
                {
                    //var registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                    var desiredProperties = new TwinCollection();
                    desiredProperties[properties.Key] = properties.Value;
                    var twin = await registryManager.GetTwinAsync(deviceId);
                    twin.Properties.Desired = desiredProperties;
                    await registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
                    return "Desired Properties updated successfully";
                }
                return "Device is disabled";
           }
        }
    } 
}
