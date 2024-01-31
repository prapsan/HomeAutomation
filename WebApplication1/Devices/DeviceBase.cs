using System;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;

namespace WebApplication1.Devices
{
    public abstract class DeviceBase : IDevice
    {
        public Guid Id { get; set; }
        public DeviceType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsOn { get; set; }

        public abstract string Status();
        public abstract void Trigger();
    }
}
