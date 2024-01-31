using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApplication1.Devices;

namespace WebApplication1.Helpers
{
    public class DeviceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Device));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            int deviceTypeValue = jo["Type"].Value<int>();

            switch (deviceTypeValue)
            {
                case (int)DeviceType.Generic:
                    return jo.ToObject<GenericDevice>(serializer);
                case (int)DeviceType.GarageDoor:
                    return jo.ToObject<GarageDoor>(serializer);
                case (int)DeviceType.Dishwasher:
                    return jo.ToObject<Dishwasher>(serializer);
                case (int)DeviceType.Lights:
                    return jo.ToObject<Lights>(serializer);
                default:
                    return null;
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
