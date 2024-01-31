using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using WebApplication1.Devices;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;

namespace WebApplication1
{
    public class DevicesDatabase : IRepository<Device>
    {
        private Dictionary<string, Device> _devices = new Dictionary<string, Device>();
        private string _filePath = "devices.json";

        public DevicesDatabase()
        {
            LoadDevices();
        }

        private void LoadDevices()
        {
            if (File.Exists(_filePath))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = File.ReadAllText(_filePath);
                JsonConverter[] converters = { new DeviceConverter() };
                _devices = JsonConvert.DeserializeObject<Dictionary<string, Device>>(json, new JsonSerializerSettings() { Converters = converters });
            }
            else
            {
                _devices = new Dictionary<string, Device>();
            }
        }

        private void SaveDevices()
        {
            string json = JsonConvert.SerializeObject(_devices);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Device> GetAll()
        {
            return _devices.Values;
        }

        public Device? GetById(Guid id)
        {
            _devices.TryGetValue(id.ToString(), out var device);
            return device;
        }

        public void Add(Device device)
        {
            _devices[device.Id.ToString()] = device;
            SaveDevices();
        }

        public void Delete(Guid id)
        {
            _devices.Remove(id.ToString());
            SaveDevices();
        }

        public Task<bool> UpdateAsync(Guid id, Device entity)
        {
            throw new NotImplementedException();
        }        
    }
}
