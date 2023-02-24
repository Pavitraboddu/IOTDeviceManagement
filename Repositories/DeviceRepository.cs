using IOTDeviceManagement.Models;
using Microsoft.Azure.Devices;

namespace IOTDeviceManagement.Repositories
{
    public class DeviceRepository
    {
        private static string connectionString = "";
        private static RegistryManager registryManager;
        public DeviceRepository()
        { 
            registryManager =  RegistryManager.CreateFromConnectionString(connectionString);
        }
        public async Task<string> AddDeviceAsync(IOTDevice iotDevice)
        {
            var device = new Device(iotDevice.Id);
            Device createDevice = await registryManager.AddDeviceAsync(device);
            return createDevice.Authentication.SymmetricKey.PrimaryKey;
        }
        public async Task<Device> GetDeviceAsync(string id)
        {
            Device device = await registryManager.GetDeviceAsync(id);
            return device;
        }   
        public async Task UpdateDeviceStatusAsync(string id,string status )
        {
            Device device = await registryManager.GetDeviceAsync(id);
            device.Status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), status,true);
            await registryManager.UpdateDeviceAsync(device);
        }
        public async Task DeleteDeviceAsync(string id)
        {
            await registryManager.RemoveDeviceAsync(id);
        }
    }
}
