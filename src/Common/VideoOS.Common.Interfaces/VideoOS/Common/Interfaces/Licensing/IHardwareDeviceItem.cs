namespace VideoOS.Common.Interfaces.Licensing;

public interface IHardwareDeviceItem
{
	string MacAddress { get; }

	string DeviceId { get; }
}
