using WebApplication1.Interfaces;

namespace WebApplication1.Devices
{
    public class Device : DeviceBase
    {
        public override string Status()
        {
            string name = GetType().Name;
            return IsOn ? $"{name} is On" : $"{name} is Off";
        }

        public override void Trigger()
        {
            string name = GetType().Name;
            System.Diagnostics.Debug.WriteLine(IsOn ? $"{name} Turning Off" : $"{name} Turning On");
            IsOn = !IsOn;
        }
    }
}
