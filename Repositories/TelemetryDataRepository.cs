﻿using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System.Text;
using IOTDeviceManagement.Models;

namespace IOTDeviceManagement.Repositories
{
    public class TelemetryDataRepository
    {
        private static string connectionString = "";
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

        public async Task<string> SendTelemetryData(string deviceId, TelemetryData telemetryData)
        {
            if (await IsDeviceAvailable(deviceId))
            {
                try
                {
                    var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, deviceId, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
                    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(telemetryData.Data));
                    message.Properties.Add("deviceId", deviceId);
                    await deviceClient.SendEventAsync(message);
                    return "data sent successfully";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return connectionString;
        }
    }
}

    
