using System.Text.Json.Serialization;

namespace EntityFrameworkProperyDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DeviceDescriptionDto deviceDescriptionDto = new DeviceDescriptionDto()
            {
                DescriptionId = "1",
                DeviceName = "name",
                Language = "zh",
            };

            //ModbusCapabilityResponseDto在Devices内部不可以声明，只有在外部声明好之后再赋值给Device
            ModbusCapabilityResponseDto modbus = new ModbusCapabilityResponseDto()
            {
                ModbusCapID = "123"
            };
            deviceDescriptionDto.SetModbusCapability(modbus);

            var ret = System.Text.Json.JsonSerializer.Serialize(deviceDescriptionDto);

            Console.WriteLine("Hello, World!");
        }
    }

    public class DeviceDescriptionDto
    {
        [JsonPropertyOrder(1)]
        public string Language { get; set; }

        [JsonPropertyOrder(2)]
        public string DescriptionId { get; set; }

        [JsonPropertyOrder(3)]
        public string DeviceName { get; set; }

        public ModbusCapabilityResponseDto ModbusCapability { get; private set; } =
            new ModbusCapabilityResponseDto();

        public void SetModbusCapability(ModbusCapabilityResponseDto modbusCapability)
        {
            ModbusCapability = modbusCapability;
        }
    }

    public class ModbusCapabilityResponseDto
    {
        public string ModbusCapID { get; set; }
    }
}
